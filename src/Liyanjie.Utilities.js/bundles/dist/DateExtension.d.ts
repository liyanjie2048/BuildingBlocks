interface Date {
    format(format?: string, weekDisplay?: WeekDisplay): string;
    addMillionSeconds(millionSeconds: number): Date;
    addSeconds(seconds: number): Date;
    addMinutes(minutes: number): Date;
    addHours(hours: number): Date;
    addDays(days: number): Date;
    addMonths(months: number): Date;
    addYears(years: number): Date;
}
interface WeekDisplay {
    sun?: string;
    mon?: string;
    tue?: string;
    wed?: string;
    thu?: string;
    fri?: string;
    sat?: string;
}
