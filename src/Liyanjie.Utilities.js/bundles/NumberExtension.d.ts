interface Number {
    plus(arg: number): number;
    minus(arg: number): number;
    multipy(arg: number): number;
    divide(arg: number): number;
    toCn(numberType?: string): string;
}
declare function __toCnNumber(number: number, uppercase: boolean): string;
