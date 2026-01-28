import { useMutation, useQuery } from "@tanstack/react-query";
import { Loader2, Send } from "lucide-react";
import { type ReactNode, useMemo, useState } from "react";
import type { CompanyContactDto } from "@/api";
import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Textarea } from "@/components/ui/textarea";
import { useAuth } from "@/contexts/auth-context";
import { callApi } from "@/server/proxy";
import { toast } from "sonner";

export function ConnectRequestDialog({
  partnerId,
  partnerName,
  children,
}: {
  partnerId: string;
  partnerName?: string;
  children: ReactNode;
}) {
  const { user } = useAuth();
  const [open, setOpen] = useState(false);
  const [contactId, setContactId] = useState<string>("");
  const [message, setMessage] = useState<string>("");

  const companyId = user?.companyId;

  const contactsQuery = useQuery({
    queryKey: ["companies", companyId, "contacts"],
    queryFn: async () => {
      if (!companyId) {
        return [] as CompanyContactDto[];
      }
      return (await callApi({
        data: {
          fn: "getApiCompaniesByIdContacts",
          args: { path: { id: companyId } },
        },
      })) as CompanyContactDto[];
    },
    enabled: Boolean(companyId),
    staleTime: 5 * 60 * 1000,
  });

  const mutation = useMutation({
    mutationFn: async () => {
      if (!companyId) {
        throw new Error("User is not linked to a company");
      }
      if (!contactId) {
        throw new Error("Please select a contact");
      }
      await callApi({
        data: {
          fn: "putApiCompaniesConnectionRequest",
          args: {
            body: {
              contactId,
              companyId,
              partnerId,
              message,
            },
          },
        },
      });
    },
    onSuccess: () => {
      toast.success("Your connect request has been sent successfully.");
      setOpen(false);
      setContactId("");
      setMessage("");
    },
    onError: () => {
      toast.error("Failed to send connect request.");
    },
  });

  const errorMessage = useMemo(() => {
    if (!mutation.isError) {
      return null;
    }
    if (mutation.error instanceof Error) {
      return mutation.error.message;
    }
    return "Failed to send connect request";
  }, [mutation.error, mutation.isError]);

  if (!companyId) {
    return null;
  }

  const contacts = contactsQuery.data || [];

  let contactPicker: ReactNode;
  if (contactsQuery.isLoading) {
    contactPicker = (
      <div className="flex items-center gap-2 text-muted-foreground text-sm">
        <Loader2 className="h-4 w-4 animate-spin" /> Loading contacts
      </div>
    );
  } else if (contactsQuery.isError) {
    contactPicker = (
      <div className="text-red-600 text-sm">Failed to load contacts</div>
    );
  } else {
    contactPicker = (
      <Select onValueChange={setContactId} value={contactId}>
        <SelectTrigger>
          <SelectValue placeholder="Select a contact" />
        </SelectTrigger>
        <SelectContent>
          {contacts.map((c) => {
            const id = c.id || "";
            const label = `${c.firstName || ""} ${c.lastName || ""}`.trim();
            const secondary = c.emailAddress || c.companyPosition || "";

            return (
              <SelectItem key={id} value={id}>
                <div className="flex flex-col">
                  <span>{label || "Contact"}</span>
                  {secondary ? (
                    <span className="text-muted-foreground text-xs">
                      {secondary}
                    </span>
                  ) : null}
                </div>
              </SelectItem>
            );
          })}
        </SelectContent>
      </Select>
    );
  }

  return (
    <Dialog
      onOpenChange={(nextOpen) => {
        setOpen(nextOpen);
        if (nextOpen) {
          mutation.reset();
          setMessage("");
        }
      }}
      open={open}
    >
      <DialogTrigger asChild>{children}</DialogTrigger>
      <DialogContent className="max-w-[640px] gap-0 overflow-hidden rounded-none border border-gray-200 bg-white p-0">
        <DialogHeader className="border-gray-100 border-b px-10 pt-8 pb-6 text-left">
          <DialogTitle className="font-serif text-3xl text-black">
            Connect with {partnerName || "partner"}
          </DialogTitle>
          <DialogDescription className="pt-1 font-bold text-[10px] text-gray-400 uppercase tracking-[0.2em]">
            Send a direct message
          </DialogDescription>
        </DialogHeader>

        <div className="space-y-7 px-10 py-8">
          <div className="space-y-2">
            <div className="font-bold text-[10px] text-gray-400 uppercase tracking-[0.2em]">
              Reply email address
            </div>
            <div className="border border-gray-200 bg-white px-4 py-3">
              {contactPicker}
            </div>
            <div className="font-light text-gray-400 text-xs italic">
              Defaults to your account email, change if you wish to receive the
              reply elsewhere.
            </div>
          </div>

          <div className="space-y-2">
            <div className="font-bold text-[10px] text-gray-400 uppercase tracking-[0.2em]">
              Message
            </div>
            <div className="border border-gray-200 bg-white">
              <Textarea
                className="min-h-[160px] resize-none rounded-none border-0 p-4 text-sm outline-none focus-visible:ring-0"
                onChange={(e) => setMessage(e.target.value)}
                placeholder={`Dear ${partnerName || "Partner"}, we are interested in collaborating on...`}
                value={message}
              />
            </div>
          </div>

          {errorMessage && (
            <div className="font-medium text-[#DB0A20] text-sm">
              {errorMessage}
            </div>
          )}
        </div>

        <DialogFooter className="border-gray-100 border-t px-10 py-6 sm:justify-between">
          <DialogClose asChild>
            <button
              className="font-bold text-[10px] text-gray-500 uppercase tracking-[0.2em] transition-colors hover:text-black"
              type="button"
            >
              Cancel
            </button>
          </DialogClose>

          <Button
            className="rounded-none bg-[#DB0A20] px-10 py-6 font-bold text-[10px] text-white uppercase tracking-[0.2em] hover:bg-[#111111]"
            disabled={
              mutation.isPending || contactsQuery.isLoading || !contactId || !message
            }
            onClick={() => mutation.mutate()}
            type="button"
          >
            {mutation.isPending ? (
              <span className="flex items-center gap-2">
                <Loader2 className="h-4 w-4 animate-spin" /> Sending connection
              </span>
            ) : (
              <span className="flex items-center gap-2">
                Send connection <Send className="h-4 w-4" />
              </span>
            )}
          </Button>
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}
