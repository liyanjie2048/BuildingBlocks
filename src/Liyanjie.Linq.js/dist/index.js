var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __spreadArray = (this && this.__spreadArray) || function (to, from, pack) {
    if (pack || arguments.length === 2) for (var i = 0, l = from.length, ar; i < l; i++) {
        if (ar || !(i in from)) {
            if (!ar) ar = Array.prototype.slice.call(from, 0, i);
            ar[i] = from[i];
        }
    }
    return to.concat(ar || Array.prototype.slice.call(from));
};
/**
 * 可枚举对象类
 */
var Enumerable = /** @class */ (function () {
    /**
     * 创建一个Enumerable对象，基于此对象可以使用LinqJs的各种方法
     * @param source 源数组
     */
    function Enumerable(source) {
        Enumerable._check(source);
        this.source = source;
    }
    Enumerable._check = function (array) {
        if (array === null || array === undefined || !Array.isArray(array))
            throw new Error('Array parameter can not be null or undefined!');
    };
    /**
     * 创建一个Enumerable对象，基于此对象可以使用LinqJs的各种方法
     * @param source 源数组
     * @returns
     */
    Enumerable.new = function (source) {
        return new Enumerable(source || []);
    };
    /**
     * 创建一个Enumerable对象，基于此对象可以使用LinqJs的各种方法
     * @param source 源数组
     */
    Enumerable.from = function (source) {
        return new Enumerable(source || []);
    };
    /**
     * 判断可枚举对象中的每一元素能够满足指定条件
     * @param predicate 条件表达式
     */
    Enumerable.prototype.all = function (predicate) {
        return this.every(predicate);
    };
    /**
     * 判断可枚举对象中的每一元素能够满足指定条件
     * @param predicate 条件表达式
     */
    Enumerable.prototype.every = function (predicate) {
        return this.source.every(predicate) === true;
    };
    /**
     * 判断可枚举对象中的任一对象能够满足指定条件
     * @param predicate
     */
    Enumerable.prototype.any = function (predicate) {
        return this.some(predicate);
    };
    /**
     * 判断可枚举对象中的任一对象能够满足指定条件
     * @param predicate
     */
    Enumerable.prototype.some = function (predicate) {
        return predicate
            ? this.source.some(predicate) === true
            : this.source.length > 0;
    };
    /**
     * 向可枚举对象的末尾追加元素
     * @param elements 元素
     */
    Enumerable.prototype.append = function () {
        var _a;
        var elements = [];
        for (var _i = 0; _i < arguments.length; _i++) {
            elements[_i] = arguments[_i];
        }
        Enumerable._check(elements);
        (_a = this.source).push.apply(_a, elements);
        return this;
    };
    /**
     * 对可枚举对象中的元素的指定属性求平均值
     * @param selector 属性表达式
     */
    Enumerable.prototype.average = function (selector) {
        return this.sum(selector) / this.source.length;
    };
    /**
     * 拼接序列
     * @param targets 目标
     */
    Enumerable.prototype.concat = function () {
        var _a;
        var targets = [];
        for (var _i = 0; _i < arguments.length; _i++) {
            targets[_i] = arguments[_i];
        }
        Enumerable._check(targets);
        return new Enumerable((_a = this.source).concat.apply(_a, targets));
    };
    /**
     * 判断可枚举对象中是否包含指定元素
     * @param element 指定元素
     * @param comparer 元素对比器
     */
    Enumerable.prototype.contains = function (element, comparer) {
        return comparer
            ? this.source.some(function (_) { return comparer(_, element); }) === true
            : this.source.indexOf(element) > -1;
    };
    /**
     * 取可枚举对象中元素的数量
     * @param predicate 条件表达式
     */
    Enumerable.prototype.count = function (predicate) {
        return predicate
            ? this.source.filter(predicate).length
            : this.source.length;
    };
    /**
     * 如果可枚举对象中元素数量为0，则返回包含默认值的新可枚举对象
     * @param defaultValue 默认值
     */
    Enumerable.prototype.defaultIfEmpty = function (defaultValue) {
        return this.isEmpty()
            ? new Enumerable(defaultValue ? [defaultValue] : [])
            : this;
    };
    /**
     * 对可枚举对象中的元素去重
     * @param comparer 元素对比器
     */
    Enumerable.prototype.distinct = function (comparer) {
        comparer = comparer || (function (item1, item2) { return item1 === item2; });
        var result = [];
        this.source.forEach(function (item) { return (result.some(function (_) { return comparer(_, item); }) === false) && result.push(item); });
        return new Enumerable(result);
    };
    /**
     * 返回可枚举对象中指定索引的元素，如果索引超出，返回undefined
     * @param index
     */
    Enumerable.prototype.elementAtOrDefault = function (index) {
        return this.source.length > index
            ? this.source[index]
            : undefined;
    };
    /**
     * 生成一个空的可枚举对象
     */
    Enumerable.empty = function () {
        return new Enumerable([]);
    };
    /**
     * 排除可枚举对象中存在于目标序列的元素
     * @param target 目标序列
     * @param comparer 元素对比器
     */
    Enumerable.prototype.except = function (target, comparer) {
        Enumerable._check(target);
        comparer = comparer || (function (item1, item2) { return item1 === item2; });
        var result = [];
        this.source.forEach(function (item) { return (target.some(function (_) { return comparer(_, item); }) === false) && result.push(item); });
        return new Enumerable(result);
    };
    /**
     * 取可枚举对象中满足条件的第一个元素，如果可枚举对象元素数量为0，则返回undefined
     * @param predicate 条件表达式
     */
    Enumerable.prototype.firstOrDefault = function (predicate) {
        var source = predicate
            ? this.source.filter(predicate)
            : this.source;
        return source.length > 0
            ? source[0]
            : undefined;
    };
    /**
     * ForEach循环
     * @param callbackFn 回掉函数
     */
    Enumerable.prototype.forEach = function (callbackFn) {
        this.source.forEach(callbackFn);
    };
    /**
     * 对可枚举对象中的元素进行分组
     * @param keySelector 属性选择器，最为分组依据
     * @param comparer 属性对比器
     */
    Enumerable.prototype.groupBy = function (keySelector, comparer) {
        comparer = comparer || (function (item1, item2) { return item1 === item2; });
        var result = [];
        this.source.forEach(function (item) {
            var key = keySelector(item);
            var array = result.filter(function (_) { return comparer(_.key, key); });
            array.length > 0
                ? array[0].source.push(item)
                : result.push(new GroupedEnumerable({ key: key, source: [item] }));
        });
        return new Enumerable(result);
    };
    /**
     * 将可枚举对象与目标序列进行融合
     * @param target 目标序列
     * @param keySelector 属性选择器
     * @param targetKeySelector 属性选择器
     * @param resultSelector 结果选择器
     * @param comparer 属性对比器
     */
    Enumerable.prototype.groupJoin = function (target, keySelector, targetKeySelector, resultSelector, comparer) {
        Enumerable._check(target);
        comparer = comparer || (function (item1, item2) { return item1 === item2; });
        var result = [];
        var _loop_1 = function (i) {
            var item1 = this_1.source[i];
            var key = keySelector(item1);
            var item2 = target.filter(function (_) { return comparer(key, targetKeySelector(_)); });
            var selected = resultSelector(item1, item2, key);
            selected && result.push(selected);
        };
        var this_1 = this;
        for (var i = 0; i < this.source.length; i++) {
            _loop_1(i);
        }
        return new Enumerable(result);
    };
    /**
     * 取可枚举对象中与目标序列的交集
     * @param target 目标序列
     * @param comparer 元素对比器
     */
    Enumerable.prototype.intersect = function (target, comparer) {
        Enumerable._check(target);
        comparer = comparer || (function (item1, item2) { return item1 === item2; });
        var result = [];
        this.source.forEach(function (item) { return (target.some(function (_) { return comparer(_, item); }) === true) && result.push(item); });
        return new Enumerable(result);
    };
    /**取可枚举对象中的第一个元素，如果可枚举对象元素数量为0，则返回undefined
     * 判断可枚举对象是否包含元素
     */
    Enumerable.prototype.isEmpty = function () {
        return this.source.length === 0;
    };
    /**
     * 以相等属性为条件将可枚举对象与目标序列融合
     * @param target 目标序列
     * @param keySelector 属性选择器
     * @param targetKeySelector 属性选择器
     * @param resultSelector 结果选择器
     * @param comparer 属性对比器
     */
    Enumerable.prototype.join = function (target, keySelector, targetKeySelector, resultSelector, comparer) {
        Enumerable._check(target);
        comparer = comparer || (function (item1, item2) { return item1 === item2; });
        var result = [];
        var _loop_2 = function (i) {
            var item1 = this_2.source[i];
            var key = keySelector(item1);
            var filteredTarget = target.filter(function (_) { return comparer(key, targetKeySelector(_)); });
            var item2 = filteredTarget.length > 0
                ? filteredTarget[0]
                : undefined;
            var selected = resultSelector(item1, item2, key);
            selected && result.push(selected);
        };
        var this_2 = this;
        for (var i = 0; i < this.source.length; i++) {
            _loop_2(i);
        }
        return new Enumerable(result);
    };
    /**
     * 取可枚举对象中满足条件的最后一个元素，如果可枚举对象元素数量为0，则返回undefined
     * @param predicate 条件表达式
     */
    Enumerable.prototype.lastOrDefault = function (predicate) {
        var source = predicate
            ? this.source.filter(predicate)
            : this.source;
        return this.source.length > 0
            ? this.source[this.source.length - 1]
            : undefined;
    };
    /**
     * 对可枚举对象中的元素的指定属性求最大值
     * @param selector 属性表达式
     */
    Enumerable.prototype.max = function (selector) {
        return this.source.length === 0
            ? 0
            : selector(this.source.sort(function (item1, item2) { return selector(item2) - selector(item1); })[0]);
    };
    /**
     * 对可枚举对象中的元素的指定属性求最小值
     * @param selector 属性表达式
     */
    Enumerable.prototype.min = function (selector) {
        return this.source.length === 0
            ? 0
            : selector(this.source.sort(function (item1, item2) { return selector(item1) - selector(item2); })[0]);
    };
    /**
     * 对可枚举对象中的元素进行排序（升序）
     * @param keySelector 属性选择器
     * @param comparer 属性对比器
     */
    Enumerable.prototype.orderBy = function (keySelector, comparer, keyEqualizer) {
        return this.__orderBy(keySelector, false, comparer, keyEqualizer);
    };
    /**
     * 对可枚举对象中的元素进行排序（降序）
     * @param keySelector
     * @param comparer
     */
    Enumerable.prototype.orderByDescending = function (keySelector, comparer, keyEqualizer) {
        return this.__orderBy(keySelector, true, comparer, keyEqualizer);
    };
    Enumerable.prototype.__orderBy = function (keySelector, descending, comparer, keyEqualizer) {
        keyEqualizer = keyEqualizer || (function (item1, item2) { return item1 === item2; });
        var keys = [];
        var group = [];
        this.source.forEach(function (item) {
            var key = keySelector(item);
            keys.indexOf(key) < 0 && keys.push(key);
            var array = group.filter(function (_) { return keyEqualizer(_.key, key); });
            array.length > 0
                ? array[0].source.push(item)
                : group.push({ key: key, source: [item] });
        });
        keys = comparer ? keys.sort(comparer) : keys.sort();
        (descending) && (keys = keys.reverse());
        var result = [];
        keys.forEach(function (item) { return result.push(new GroupedEnumerable(group.filter(function (_) { return keyEqualizer(item, _.key); })[0])); });
        keys = undefined;
        group = undefined;
        return new OrderedEnumerable(result);
    };
    /**
     * 向可枚举对象的开头添加元素
     * @param elements 目标元素
     */
    Enumerable.prototype.prepend = function () {
        var elements = [];
        for (var _i = 0; _i < arguments.length; _i++) {
            elements[_i] = arguments[_i];
        }
        Enumerable._check(elements);
        this.source = new (Array.bind.apply(Array, __spreadArray(__spreadArray([void 0], elements, false), this.source, false)))();
        return this;
    };
    /**
     * 生成一个新的数值型可枚举对象
     * @param start 起始值
     * @param count 元素量
     */
    Enumerable.range = function (start, count) {
        var result = [];
        for (var i = 0; i < count; i++) {
            result.push(start + i);
        }
        return new NumberEnumerable(result);
    };
    /**
     * 向可枚举对象中重复添加元素
     * @param element 目标元素
     * @param count 添加数量
     */
    Enumerable.repeat = function (element, count) {
        var result = [];
        for (var i = 0; i < count; i++) {
            result.push(element);
        }
        return new Enumerable(result);
    };
    /**
     * 对可枚举对象中的元素反向排序
     */
    Enumerable.prototype.reverse = function () {
        return new Enumerable(new (Array.bind.apply(Array, __spreadArray([void 0], this.source, false)))().reverse());
    };
    /**
     * 遍历可枚举对象并生成一个新的可枚举对象
     * @param selector 元素选择器
     */
    Enumerable.prototype.select = function (selector) {
        var result = [];
        this.source.forEach(function (item, index) {
            var selected = selector(item, index);
            selected && result.push(selected);
        });
        return new Enumerable(result);
    };
    /**
     * 遍历可枚举对象并生成一个新的可枚举对象
     * @param resultSelector 序列选择器
     */
    Enumerable.prototype.selectMany = function (resultSelector) {
        var result = [];
        this.source.forEach(function (item, index) {
            var selected = resultSelector(item, index);
            selected && result.push.apply(result, selected);
        });
        return new Enumerable(result);
    };
    /**
     * 对比可枚举对象与目标序列中的每一个元素
     * @param target 目标序列
     * @param comparer 元素对比器
     */
    Enumerable.prototype.sequenceEqual = function (target, comparer) {
        Enumerable._check(target);
        if (this.source.length !== target.length)
            return false;
        comparer = comparer || (function (item1, item2) { return item1 === item2; });
        var result = true;
        for (var i = 0; i < this.source.length; i++) {
            var item1 = this.source[i];
            var item2 = target.length > i ? target[i] : undefined;
            if (!comparer(item1, item2)) {
                result = false;
                break;
            }
        }
        comparer = undefined;
        return result;
    };
    /**
     * 跳过指定数量的元素取剩下的元素
     * @param count 数量
     */
    Enumerable.prototype.skip = function (count) {
        var result = [];
        this.source.forEach(function (item, index) { return (index >= count) && result.push(item); });
        return new Enumerable(result);
    };
    /**
     * 跳过满足条件的元素取第一组连续元素
     * @param predicate 条件表达式
     */
    Enumerable.prototype.skipWhile = function (predicate) {
        var result = [];
        var flag = false;
        for (var i = 0; i < this.source.length; i++) {
            var item = this.source[i];
            if (!predicate(item, i)) {
                (!flag) && (flag = true);
                result.push(item);
            }
            else if (flag)
                break;
        }
        this.source.forEach(function (item, index) { return (!predicate(item, index)) && result.push(item); });
        return new Enumerable(result);
    };
    /**
     * 对可枚举对象中元素的属性求和
     * @param selector 属性选择器
     */
    Enumerable.prototype.sum = function (selector) {
        if (this.source.length === 0)
            return 0;
        var sum = 0;
        this.source.forEach(function (item) { return sum += selector(item); });
        return sum;
    };
    /**
     * 取可枚举对象中指定数量的元素
     * @param count 数量
     */
    Enumerable.prototype.take = function (count) {
        var result = [];
        this.source.forEach(function (item, index) { return (index < count) && result.push(item); });
        return new Enumerable(result);
    };
    /**
     * 取可枚举对象满足条件的第一组连续元素
     * @param predicate
     */
    Enumerable.prototype.takeWhile = function (predicate) {
        var result = [];
        var flag = false;
        for (var i = 0; i < this.source.length; i++) {
            var item = this.source[i];
            if (predicate(item, i)) {
                (!flag) && (flag = true);
                result.push(item);
            }
            else if (flag)
                break;
        }
        return new Enumerable(result);
    };
    /**
     * 将可枚举对象中的元素输出为数组
     */
    Enumerable.prototype.toArray = function () {
        return this.source;
    };
    /**
     * 将可枚举对象转换为键值对可枚举对象
     * @param keySelector 属性选择器
     * @param valueSelector 属性选择器
     */
    Enumerable.prototype.toDictionary = function (keySelector, valueSelector) {
        var result = {};
        this.source.forEach(function (item) {
            var key = keySelector(item);
            (!result[key]) && (result[key] = valueSelector(item));
        });
        return result;
    };
    /**
     * 将可枚举对象中的元素转换为字符串并拼接
     * @param separator 拼接符
     * @param stringSelector 字符串选择器
     */
    Enumerable.prototype.toString = function (separator, stringSelector) {
        (separator === null || separator === undefined) && (separator = ',');
        stringSelector = stringSelector || (function (item) { return item.toString(); });
        var result = '';
        this.source.forEach(function (item) { return result += stringSelector(item) + separator; });
        return result.substring(0, result.length - separator.length);
    };
    /**
     * 连接可枚举对象与目标序列，并排除目标序列在当前可枚举对象中已存在的元素
     * @param target
     * @param comparer
     */
    Enumerable.prototype.union = function (target, comparer) {
        Enumerable._check(target);
        comparer = comparer || (function (item1, item2) { return item1 === item2; });
        var result = new (Array.bind.apply(Array, __spreadArray([void 0], this.source, false)))();
        target.forEach(function (item) { return (result.some(function (item2) { return comparer(item, item2); }) === false) && result.push(item); });
        comparer = undefined;
        return new Enumerable(result);
    };
    /**
     * 取可枚举对象中满足条件的元素
     * @param predicate
     */
    Enumerable.prototype.where = function (predicate) {
        var result = [];
        this.source.forEach(function (item, index) { return (predicate(item, index)) && result.push(item); });
        return new Enumerable(result);
    };
    /**
     * 以索引位置为条件将可枚举对象与目标序列融合
     * @param target
     * @param resultSelector
     */
    Enumerable.prototype.zip = function (target, resultSelector) {
        Enumerable._check(target);
        var result = [];
        for (var i = 0; i < this.source.length; i++) {
            var item1 = this.source[i];
            var item2 = target.length > i
                ? target[i]
                : undefined;
            result.push(resultSelector(item1, item2, i));
        }
        return new Enumerable(result);
    };
    return Enumerable;
}());
export { Enumerable };
/**
 * 已分组的可枚举集合数据
 */
var GroupedEnumerable = /** @class */ (function (_super) {
    __extends(GroupedEnumerable, _super);
    function GroupedEnumerable(groupedSource) {
        var _this = _super.call(this, groupedSource.source) || this;
        _this.key = groupedSource.key;
        return _this;
    }
    return GroupedEnumerable;
}(Enumerable));
export { GroupedEnumerable };
/**
 * 数值型可枚举对象类
 */
var NumberEnumerable = /** @class */ (function (_super) {
    __extends(NumberEnumerable, _super);
    function NumberEnumerable() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    /**
     * 对可枚举对象求平均值
     */
    NumberEnumerable.prototype.average = function () {
        return this.sum() / this.source.length;
    };
    /**
     * 返回可枚举对象中指定索引的元素，如果索引超出，返回0
     * @param index
     */
    NumberEnumerable.prototype.elementAtOrDefault = function (index) {
        return this.source.length > index
            ? this.source[index]
            : 0;
    };
    /**
     * 取可枚举对象中第一个元素，如果可枚举对象元素数量为0，则返回0
     * @param predicate
     */
    NumberEnumerable.prototype.firstOrDefault = function (predicate) {
        return this.source.length > 0
            ? this.source[0]
            : 0;
    };
    /**
     * 取可枚举对象中的最后一个元素，如果可枚举对象元素数量为0，则返回0
     * @param predicate
     */
    NumberEnumerable.prototype.lastOrDefault = function (predicate) {
        return this.source.length > 0
            ? this.source[this.source.length - 1]
            : 0;
    };
    /**
     * 对可枚举对象中的元素求最大值
     */
    NumberEnumerable.prototype.max = function () {
        return this.source.length === 0
            ? 0
            : this.source.sort(function (item1, item2) { return item2 - item1; })[0];
    };
    /**
     * 对可枚举对象中的元素求最小值
     */
    NumberEnumerable.prototype.min = function () {
        return this.source.length === 0
            ? 0
            : this.source.sort(function (item1, item2) { return item1 - item2; })[0];
    };
    /**
     * 对可枚举对象中元素求和
     */
    NumberEnumerable.prototype.sum = function () {
        if (this.source.length === 0)
            return 0;
        var sum = 0;
        this.source.forEach(function (item) { return sum += item; });
        return sum;
    };
    return NumberEnumerable;
}(Enumerable));
export { NumberEnumerable };
/**
 * 已排序的可枚举数据集合
 */
var OrderedEnumerable = /** @class */ (function (_super) {
    __extends(OrderedEnumerable, _super);
    function OrderedEnumerable(groupedSource) {
        var _this = _super.call(this, Enumerable.new(groupedSource).selectMany(function (item) { return item.source; }).toArray()) || this;
        _this.groupedSource = groupedSource;
        return _this;
    }
    /**
     * 对已排序的可枚举对象在保持原有排序结果的情况下再次排序（升序）
     * @param keySelector 属性选择器
     * @param comparer 属性对比器
     */
    OrderedEnumerable.prototype.thenBy = function (keySelector, comparer, keyEqualizer) {
        return this.__thenBy(keySelector, false, comparer, keyEqualizer);
    };
    /**
     * 对已排序的可枚举对象在保持原有排序结果的情况下再次排序（降序）
     * @param keySelector 属性选择器
     * @param comparer 属性对比器
     */
    OrderedEnumerable.prototype.thenByDescending = function (keySelector, comparer, keyEqualizer) {
        return this.__thenBy(keySelector, true, comparer, keyEqualizer);
    };
    OrderedEnumerable.prototype.__thenBy = function (keySelector, descending, comparer, keyEqualizer) {
        keyEqualizer = keyEqualizer || (function (item1, item2) { return item1 === item2; });
        var result = [];
        this.groupedSource.forEach(function (item) {
            var enumerable = descending
                ? item.orderByDescending(keySelector, comparer, keyEqualizer)
                : item.orderBy(keySelector, comparer, keyEqualizer);
            enumerable.groupedSource.forEach(function (item2) { return result.push(new GroupedEnumerable({ key: item.key + ':' + item2.key, source: item2.source })); });
        });
        return new OrderedEnumerable(result);
    };
    return OrderedEnumerable;
}(Enumerable));
export { OrderedEnumerable };
//# sourceMappingURL=index.js.map