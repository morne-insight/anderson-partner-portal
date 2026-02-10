import { createServerFn } from '@tanstack/react-start'
import {
	getApiUserDetail,
	postApiAccountConfirmEmail,
	postApiAccountForgotPassword,
	postApiAccountLogin,
	postApiAccountLogout,
	postApiAccountRegister,
	postApiAccountResendConfirmationEmail,
	postApiAccountResetPassword,
	putApiInvitesAccept,
} from '../api'
import { useAppSession } from '../utils/session'

// Login server function
export const loginFn = createServerFn({ method: 'POST' })
	.inputValidator((data: { email: string; password: string }) => data)
	.handler(async ({ data }) => {
		try {
			const response = await postApiAccountLogin({
				body: data,
			})

			if (!response.data) {
				return { error: 'Invalid credentials' }
			}

			console.log('login result', response.data)

			const userDetailResponse = await getApiUserDetail({
				headers: {
					Authorization: `Bearer ${response.data.authenticationToken}`,
				},
			})

			if (!userDetailResponse.data) {
				return { error: 'Failed to get user details' }
			}

			// Create session with user data and tokens
			const session = await useAppSession()
			await session.update({
				userId: response.data.userId?.toString() ?? undefined, // or extract user ID from JWT
				userName: response.data.userName ?? undefined,
				email: data.email,
				companyId: userDetailResponse.data.companyId?.toString() ?? undefined,
				companyName: userDetailResponse.data.companyName ?? undefined,
				companies: userDetailResponse.data.companies ?? undefined,

				accessToken: response.data.authenticationToken ?? undefined,
				accessTokenExpiresAt: response.data.expiresIn
					? Date.now() + response.data.expiresIn * 1000
					: undefined,
				refreshToken: response.data.refreshToken ?? undefined,
			})

			// Return success instead of throwing redirect
			return { success: true }
		} catch (error) {
			return { error: 'Invalid credentials' }
		}
	})

// Register server function
export const registerFn = createServerFn({ method: 'POST' })
	.inputValidator((data: { userName: string; email: string; password: string }) => data)
	.handler(async ({ data }) => {
		try {
			const result = await postApiAccountRegister({
				body: data,
			})

			if (result.error) {
				const errorMessages = Object.values(result.error).flat();
				const errorMessage = errorMessages.length > 0 ? errorMessages[0] : 'Registration failed';
				return { success: false, error: errorMessage as string }
			}

			return { success: true }
		} catch (error) {
			console.log(String(error))
			return { success: false, error: 'Registration failed' }
		}
	})

// Logout server function
export const logoutFn = createServerFn({ method: 'POST' }).handler(async () => {
	const session = await useAppSession()

	// Call API logout if needed
	try {
		await postApiAccountLogout()
	} catch (error) {
		// Continue with local logout even if API call fails
	}

	await session.clear()
	return { success: true }
})

// Get current user
export const getCurrentUserFn = createServerFn({ method: 'GET' }).handler(async () => {
	const session = await useAppSession()
	const userId = session.data.userId

	if (!userId) {
		return null
	}

	// Return user data from session or fetch from API
	return {
		userId: session.data.userId,
		userName: session.data.userName,
		companyId: session.data.companyId,
		companyName: session.data.companyName,
		companies: session.data.companies,
		email: session.data.email!,
	}
})

// Confirm user email
export const confirmEmailfn = createServerFn({ method: 'POST' })
	.inputValidator((data: { userId: string; code: string }) => data)
	.handler(async ({ data }) => {
		try {
			const result = await postApiAccountConfirmEmail({
				body: data,
			});

			console.log("confirmEmailFn", JSON.stringify(result, null, 2));
			
			if (result.error) {
				const errorMessages = Object.values(result.error).flat();
				const errorMessage = errorMessages.length > 0 ? errorMessages[0] : 'Email confirmation failed';
				return { success: false, error: errorMessage as string }
			}
			
			return { success: true }
		} catch (error) {
			return { success: false, error: 'Email confirmation failed' }
		}
	})

// Request a password reset
export const forgotPasswordFn = createServerFn({ method: 'POST' })
	.inputValidator((data: { email: string }) => data)
	.handler(async ({ data }) => {
		try {
			await postApiAccountForgotPassword({
				body: data,
			})

			return { success: true }
		} catch (error) {
			console.log(String(error))
			return { success: false, error: 'Password reset request failed' }
		}
	})

// Reset the password
export const resetPasswordFn = createServerFn({ method: 'POST' })
	.inputValidator((data: { email: string; newPassword: string; resetCode: string }) => data)
	.handler(async ({ data }) => {
		try {
			const response = await postApiAccountResetPassword({
				body: data,
			})

			if (response.error) {
				return { success: false, error: response.error.errors?.[0] || 'Password reset failed' }
			}

			return { success: true }
		} catch (error) {
			console.log(String(error))
			return { success: false, error: 'Password reset failed' }
		}
	});

	// Accept invitation
export const acceptInvitationFn = createServerFn({ method: 'POST' })
	.inputValidator((data: { invitationId: string, userId: string }) => data)
	.handler(async ({ data }) => {
		try {
			
			const result = await putApiInvitesAccept({
				body: {
					id: data.invitationId,
					userId: data.userId,
				},
			});

			if(result.error) {
				return { success: false, error: result.error.detail || 'Invitation acceptance failed' }
			}
			
			return { success: true }
		} catch (error) {
			return { success: false, error: 'Invitation acceptance failed' }
		}
	});
	
export const removeCompanyFromSessionFn = createServerFn({ method: 'POST' })
	.inputValidator((data: { companyId: string }) => data)
	.handler(async ({ data }) => {
		const session = await useAppSession()

		const companies = session.data.companies?.filter((company) => company.id !== data.companyId) || [];
		const companyId = session.data.companyId === data.companyId ? undefined : companies[0]?.id;
		const companyName = session.data.companyId === data.companyId ? undefined : companies[0]?.name;

		await session.update({
			...session.data,
			companyId,
			companyName,
			companies,
		})

		return { success: true }
	});

// Resend confirmation email
export const resendConfirmationFn = createServerFn({ method: 'POST' })
	.inputValidator((data: { email: string }) => data)
	.handler(async ({ data }) => {
		try {
			const result = await postApiAccountResendConfirmationEmail({
				body: data
			})

			console.log('resend result', JSON.stringify(result, null, 2));

			if (result.error) {
				const errorMessages = Object.values(result.error).flat();
				const errorMessage = errorMessages.length > 0 ? errorMessages[0] : 'Resend confirmation failed';
				return { success: false, error: errorMessage as string }
			}
			
			return { success: true }
		} catch (error) {
			return { success: false, error: 'Resend confirmation failed' }
		}
	});

