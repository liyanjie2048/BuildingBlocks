﻿import { NIL, v4, parse, validate } from 'uuid';

/**
 * Guid
 */
export class Guid {
    private _uuid: string = NIL();

    constructor(input?: string) {
        if (input && typeof (input) === 'string')
            this._uuid = input;
        else
            this._uuid = NIL();
    }

    /**
     * 返回一个值，该值指示 Guid 的两个实例是否表示同一个值
     * @param other
     * @returns
     */
    equals(other: any): boolean {
        if (typeof other === 'string')
            return this._uuid === other;
        else
            return this._uuid === other?.toString();
    };

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
    format(format?: string): string {
        if (format)
            switch (format) {
                case 'N':
                    return this._uuid.replace(/,/g, '');
                case 'D':
                    return this._uuid;
                case 'B':
                    return `{${this._uuid}}`;
                case 'P':
                    return `(${this._uuid})`;
                default:
                    throw new Error("Parameter “format” must be one of N|D|B|P]");
            }
        else
            return this._uuid;
    }

    static isGuid(input: string | Guid): boolean {
        if (typeof input === 'string')
            return validate(input);
        else
            return validate(input?.toString());
    }

    static parse(input: string): Guid | undefined {
        if (this.isGuid(input))
            return new Guid(input);
        else
            return undefined;
    }

    /**
     * Guid 类的默认实例，其值保证均为零
     */
    static empty: string = NIL();

    /**
     * 初始化 Guid 类的一个新实例
     */
    static newGuid(): string {
        return v4();
    }
}