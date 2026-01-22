// Quarterly Report Types
export interface QuarterlyReport {
  id: string;
  companyId: string;
  year: number;
  quarter: 'Q1' | 'Q2' | 'Q3' | 'Q4';
  isSubmitted: boolean;
  createdDate: Date;
  submittedDate?: Date;
  lastModifiedDate: Date;
  
  // Financial Data
  revenue: number;
  expenses: number;
  netIncome: number;
  
  // Operational Metrics
  employeeCount: number;
  newClients: number;
  projectsCompleted: number;
  
  // Strategic Information
  keyAchievements: string;
  challenges: string;
  nextQuarterGoals: string;
  
  // Market Information
  marketConditions: string;
  competitivePosition: string;
  
  // Additional Notes
  additionalNotes?: string;
}

export interface CreateQuarterlyReportCommand {
  companyId: string;
  year: number;
  quarter: 'Q1' | 'Q2' | 'Q3' | 'Q4';
  revenue: number;
  expenses: number;
  netIncome: number;
  employeeCount: number;
  newClients: number;
  projectsCompleted: number;
  keyAchievements: string;
  challenges: string;
  nextQuarterGoals: string;
  marketConditions: string;
  competitivePosition: string;
  additionalNotes?: string;
}

export interface UpdateQuarterlyReportCommand extends CreateQuarterlyReportCommand {
  id: string;
}

export interface SubmitQuarterlyReportCommand {
  id: string;
}

export interface QuarterlyReportListItem {
  id: string;
  year: number;
  quarter: 'Q1' | 'Q2' | 'Q3' | 'Q4';
  isSubmitted: boolean;
  createdDate: Date;
  submittedDate?: Date;
  revenue: number;
  netIncome: number;
}

export const QUARTERS = ['Q1', 'Q2', 'Q3', 'Q4'] as const;
export const CURRENT_YEAR = new Date().getFullYear();
export const AVAILABLE_YEARS = Array.from({ length: 10 }, (_, i) => CURRENT_YEAR - i);
