/**
 * Guid
 */
export declare class Guid {
    private _uuid;
    constructor(input?: string);
    /**
     * 返回一个值，该值指示 Guid 的两个实例是否表示同一个值
     * @param other
     * @returns
     */
    equals(other: Guid | string): boolean;
    /**
     * 返回 Guid 类的此实例值的 String 表示形式。
     * 根据所提供的格式说明符，返回此 Guid 实例值的 String 表示形式。
     * N  32 位： xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
     * D  由连字符分隔的 32 位数字 xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
     * B  括在大括号中、由连字符分隔的 32 位数字：{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}
     * P  括在圆括号中、由连字符分隔的 32 位数字：(xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)
     * @param format
     * @returns
     */
    format(format?: string): string;
    static parse(input: string): Guid | undefined;
    /**
     * Guid 类的默认实例，其值保证均为零
     */
    static empty: Guid;
    /**
     * Guid 空字符串：“00000000-0000-0000-0000-000000000000”
     */
    static emptyStr: string;
    /**
     * 初始化 Guid 类的一个新实例
     */
    static new(): Guid;
    /**
     * Guid 字符串：“xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx”
     * @returns
     */
    static newStr(): string;
}
