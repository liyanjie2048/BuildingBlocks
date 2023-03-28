import { NIL, v4, validate } from 'uuid';
/**
 * Guid
 */
export var Guid = /** @class */ (function () {
    function Guid(input) {
        this._uuid = NIL;
        if (input && typeof (input) === 'string' && validate(input))
            this._uuid = input;
        else
            this._uuid = NIL;
    }
    /**
     * 返回一个值，该值指示 Guid 的两个实例是否表示同一个值
     * @param other
     * @returns
     */
    Guid.prototype.equals = function (other) {
        if (typeof other === 'string')
            return this._uuid === other;
        else
            return this.format() === other.format();
    };
    ;
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
    Guid.prototype.format = function (format) {
        if (format) {
            switch (format) {
                case 'N':
                    return this._uuid.replace(/-/g, '');
                case 'D':
                    return this._uuid;
                case 'B':
                    return "{".concat(this._uuid, "}");
                case 'P':
                    return "(".concat(this._uuid, ")");
                default:
                    throw new Error("Parameter “format” must be one of N|D|B|P]");
            }
        }
        else
            return this._uuid;
    };
    Guid.parse = function (input) {
        if (validate(input))
            return new Guid(input);
        else
            return undefined;
    };
    /**
     * 初始化 Guid 类的一个新实例
     */
    Guid.newGuid = function () {
        return new Guid(v4());
    };
    /**
     * Guid 字符串：“xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx”
     * @returns
     */
    Guid.newGuidStr = function () {
        return v4();
    };
    /**
     * Guid 类的默认实例，其值保证均为零
     */
    Guid.empty = new Guid();
    /**
     * Guid 空字符串：“00000000-0000-0000-0000-000000000000”
     */
    Guid.emptyStr = NIL;
    return Guid;
}());
//# sourceMappingURL=Guid.js.map