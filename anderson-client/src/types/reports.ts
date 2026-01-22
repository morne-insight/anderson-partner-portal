
// Helper functions
export const getQuarterLabel = (quarter: ReportQuarter): string => {
  return `Q${quarter}`;
};

export const getQuarterNumber = (quarterLabel: string): ReportQuarter => {
  return parseInt(quarterLabel.replace('Q', '')) as ReportQuarter;
};

// Constants
export const QUARTERS: ReportQuarter[] = [1, 2, 3, 4];
export const QUARTER_LABELS = ['Q1', 'Q2', 'Q3', 'Q4'] as const;
export const CURRENT_YEAR = new Date().getFullYear();
export const AVAILABLE_YEARS = Array.from(
  { length: 10 }, 
  (_, i) => CURRENT_YEAR - i
);

export enum ReportQuarter {
    Q1 = 1,
    Q2 = 2,
    Q3 = 3,
    Q4 = 4
}

export enum PartnerStatus {
    Hired = 1,
    Promoted = 2,
    Terminated = 3
}
