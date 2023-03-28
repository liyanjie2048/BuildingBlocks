"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Guid = void 0;
var uuid_1 = require("uuid");
/**
 * Guid
 */
var Guid = /** @class */ (function () {
    function Guid(input) {
        this._uuid = (0, uuid_1.NIL)();
        if (input && typeof (input) === 'string')
            this._uuid = input;
        else
            this._uuid = (0, uuid_1.NIL)();
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
            return this._uuid === (other === null || other === void 0 ? void 0 : other.toString());
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
        if (format)
            switch (format) {
                case 'N':
                    return this._uuid.replace(/,/g, '');
                case 'D':
                    return this._uuid;
                case 'B':
                    return "{".concat(this._uuid, "}");
                case 'P':
                    return "(".concat(this._uuid, ")");
                default:
                    throw new Error("Parameter “format” must be one of N|D|B|P]");
            }
        else
            return this._uuid;
    };
    Guid.isGuid = function (input) {
        if (typeof input === 'string')
            return (0, uuid_1.validate)(input);
        else
            return (0, uuid_1.validate)(input === null || input === void 0 ? void 0 : input.toString());
    };
    Guid.parse = function (input) {
        if (this.isGuid(input))
            return new Guid(input);
        else
            return undefined;
    };
    /**
     * 初始化 Guid 类的一个新实例
     */
    Guid.newGuid = function () {
        return (0, uuid_1.v4)();
    };
    /**
     * Guid 类的默认实例，其值保证均为零
     */
    Guid.empty = (0, uuid_1.NIL)();
    return Guid;
}());
exports.Guid = Guid;
//# sourceMappingURL=Guid.js.map