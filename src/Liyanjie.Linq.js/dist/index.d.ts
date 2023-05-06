/**
 * 可枚举对象类
 */
export declare class Enumerable<T> {
    /**
    *源数组，所有的操作基于此
    */
    source: T[];
    /**
     * 创建一个Enumerable对象，基于此对象可以使用LinqJs的各种方法
     * @param source 源数组
     */
    constructor(source: T[]);
    private static _check;
    /**
     * 创建一个Enumerable对象，基于此对象可以使用LinqJs的各种方法
     * @param source 源数组
     * @returns
     */
    static new<TSource>(source?: TSource[]): Enumerable<TSource>;
    /**
     * 创建一个Enumerable对象，基于此对象可以使用LinqJs的各种方法
     * @param source 源数组
     */
    static from<TSource>(source?: TSource[]): Enumerable<TSource>;
    /**
     * 判断可枚举对象中的每一元素能够满足指定条件
     * @param predicate 条件表达式
     */
    all(predicate: (item: T) => boolean): boolean;
    /**
     * 判断可枚举对象中的每一元素能够满足指定条件
     * @param predicate 条件表达式
     */
    every(predicate: (item: T) => boolean): boolean;
    /**
     * 判断可枚举对象中的任一对象能够满足指定条件
     * @param predicate
     */
    any(predicate?: (item: T) => boolean): boolean;
    /**
     * 判断可枚举对象中的任一对象能够满足指定条件
     * @param predicate
     */
    some(predicate?: (item: T) => boolean): boolean;
    /**
     * 向可枚举对象的末尾追加元素
     * @param elements 元素
     */
    append(...elements: T[]): Enumerable<T>;
    /**
     * 对可枚举对象中的元素的指定属性求平均值
     * @param selector 属性表达式
     */
    average(selector: (item: T) => number): number;
    /**
     * 拼接序列
     * @param targets 目标
     */
    concat(...targets: T[][]): Enumerable<T>;
    /**
     * 判断可枚举对象中是否包含指定元素
     * @param element 指定元素
     * @param comparer 元素对比器
     */
    contains(element: T, comparer?: (item1: T, item2: T) => boolean): boolean;
    /**
     * 取可枚举对象中元素的数量
     * @param predicate 条件表达式
     */
    count(predicate?: (item: T) => boolean): number;
    /**
     * 如果可枚举对象中元素数量为0，则返回包含默认值的新可枚举对象
     * @param defaultValue 默认值
     */
    defaultIfEmpty(defaultValue?: T): Enumerable<T>;
    /**
     * 对可枚举对象中的元素去重
     * @param comparer 元素对比器
     */
    distinct(comparer?: (item1: T, item2: T) => boolean): Enumerable<T>;
    /**
     * 返回可枚举对象中指定索引的元素，如果索引超出，返回undefined
     * @param index
     */
    elementAtOrDefault(index: number): T | undefined;
    /**
     * 生成一个空的可枚举对象
     */
    static empty<TSource>(): Enumerable<TSource>;
    /**
     * 排除可枚举对象中存在于目标序列的元素
     * @param target 目标序列
     * @param comparer 元素对比器
     */
    except(target: T[], comparer?: (item1: T, item2: T) => boolean): Enumerable<T>;
    /**
     * 取可枚举对象中满足条件的第一个元素，如果可枚举对象元素数量为0，则返回undefined
     * @param predicate 条件表达式
     */
    firstOrDefault(predicate?: (item: T) => boolean): T | undefined;
    /**
     * ForEach循环
     * @param callbackFn 回掉函数
     */
    forEach(callbackFn: (value: T, index: number) => void): void;
    /**
     * 对可枚举对象中的元素进行分组
     * @param keySelector 属性选择器，最为分组依据
     * @param comparer 属性对比器
     */
    groupBy<TKey>(keySelector: (item: T) => TKey, comparer?: (item1: TKey, item2: TKey) => boolean): Enumerable<GroupedEnumerable<T, TKey>>;
    /**
     * 将可枚举对象与目标序列进行融合
     * @param target 目标序列
     * @param keySelector 属性选择器
     * @param targetKeySelector 属性选择器
     * @param resultSelector 结果选择器
     * @param comparer 属性对比器
     */
    groupJoin<TTarget, TKey, TResult>(target: TTarget[], keySelector: (item: T) => TKey, targetKeySelector: (item: TTarget) => TKey, resultSelector: (item1: T, item2: TTarget[], key?: TKey) => TResult, comparer?: (item1: TKey, item2: TKey) => boolean): Enumerable<TResult>;
    /**
     * 取可枚举对象中与目标序列的交集
     * @param target 目标序列
     * @param comparer 元素对比器
     */
    intersect(target: T[], comparer?: (item1: T, item2: T) => boolean): Enumerable<T>;
    /**取可枚举对象中的第一个元素，如果可枚举对象元素数量为0，则返回undefined
     * 判断可枚举对象是否包含元素
     */
    isEmpty(): boolean;
    /**
     * 以相等属性为条件将可枚举对象与目标序列融合
     * @param target 目标序列
     * @param keySelector 属性选择器
     * @param targetKeySelector 属性选择器
     * @param resultSelector 结果选择器
     * @param comparer 属性对比器
     */
    join<TTarget, TKey, TResult>(target: TTarget[], keySelector: (item: T) => TKey, targetKeySelector: (item: TTarget) => TKey, resultSelector: (item1: T, item2?: TTarget, key?: TKey) => TResult, comparer?: (item1: TKey, item2: TKey) => boolean): Enumerable<TResult>;
    /**
     * 取可枚举对象中满足条件的最后一个元素，如果可枚举对象元素数量为0，则返回undefined
     * @param predicate 条件表达式
     */
    lastOrDefault(predicate?: (item: T) => boolean): T | undefined;
    /**
     * 对可枚举对象中的元素的指定属性求最大值
     * @param selector 属性表达式
     */
    max(selector: (item: T) => number): number;
    /**
     * 对可枚举对象中的元素的指定属性求最小值
     * @param selector 属性表达式
     */
    min(selector: (item: T) => number): number;
    /**
     * 对可枚举对象中的元素进行排序（升序）
     * @param keySelector 属性选择器
     * @param comparer 属性对比器
     */
    orderBy<TKey>(keySelector: (item: T) => TKey, comparer?: (item1: TKey, item2: TKey) => number, keyEqualizer?: (item1: TKey, item2: TKey) => boolean): OrderedEnumerable<T>;
    /**
     * 对可枚举对象中的元素进行排序（降序）
     * @param keySelector
     * @param comparer
     */
    orderByDescending<TKey>(keySelector: (item: T) => TKey, comparer?: (item1: TKey, item2: TKey) => number, keyEqualizer?: (item1: TKey, item2: TKey) => boolean): OrderedEnumerable<T>;
    private __orderBy;
    /**
     * 向可枚举对象的开头添加元素
     * @param elements 目标元素
     */
    prepend(...elements: T[]): Enumerable<T>;
    /**
     * 生成一个新的数值型可枚举对象
     * @param start 起始值
     * @param count 元素量
     */
    static range(start: number, count: number): NumberEnumerable;
    /**
     * 向可枚举对象中重复添加元素
     * @param element 目标元素
     * @param count 添加数量
     */
    static repeat<TSource>(element: TSource, count: number): Enumerable<TSource>;
    /**
     * 对可枚举对象中的元素反向排序
     */
    reverse(): Enumerable<T>;
    /**
     * 遍历可枚举对象并生成一个新的可枚举对象
     * @param selector 元素选择器
     */
    select<TResult>(selector: (item: T, index?: number) => TResult): Enumerable<TResult>;
    /**
     * 遍历可枚举对象并生成一个新的可枚举对象
     * @param resultSelector 序列选择器
     */
    selectMany<TResult>(resultSelector: (item: T, index?: number) => TResult[]): Enumerable<TResult>;
    /**
     * 对比可枚举对象与目标序列中的每一个元素
     * @param target 目标序列
     * @param comparer 元素对比器
     */
    sequenceEqual(target: T[], comparer?: (item1: T, item2?: T) => boolean): boolean;
    /**
     * 跳过指定数量的元素取剩下的元素
     * @param count 数量
     */
    skip(count: number): Enumerable<T>;
    /**
     * 跳过满足条件的元素取第一组连续元素
     * @param predicate 条件表达式
     */
    skipWhile(predicate: (item: T, index?: number) => boolean): Enumerable<T>;
    /**
     * 对可枚举对象中元素的属性求和
     * @param selector 属性选择器
     */
    sum(selector: (item: T) => number): number;
    /**
     * 取可枚举对象中指定数量的元素
     * @param count 数量
     */
    take(count: number): Enumerable<T>;
    /**
     * 取可枚举对象满足条件的第一组连续元素
     * @param predicate
     */
    takeWhile(predicate: (item: T, index?: number) => boolean): Enumerable<T>;
    /**
     * 将可枚举对象中的元素输出为数组
     */
    toArray(): T[];
    /**
     * 将可枚举对象转换为键值对可枚举对象
     * @param keySelector 属性选择器
     * @param valueSelector 属性选择器
     */
    toDictionary<TValue>(keySelector: (item: T) => string, valueSelector: (item: T) => TValue): {
        [key: string]: TValue;
    };
    /**
     * 将可枚举对象中的元素转换为字符串并拼接
     * @param separator 拼接符
     * @param stringSelector 字符串选择器
     */
    toString(separator?: string, stringSelector?: (item: T) => string): string;
    /**
     * 连接可枚举对象与目标序列，并排除目标序列在当前可枚举对象中已存在的元素
     * @param target
     * @param comparer
     */
    union(target: T[], comparer?: (item1: T, item2: T) => boolean): Enumerable<T>;
    /**
     * 取可枚举对象中满足条件的元素
     * @param predicate
     */
    where(predicate: (item: T, index?: number) => boolean): Enumerable<T>;
    /**
     * 以索引位置为条件将可枚举对象与目标序列融合
     * @param target
     * @param resultSelector
     */
    zip<TTarget, TResult>(target: TTarget[], resultSelector: (item: T, item2?: TTarget, index?: number) => TResult): Enumerable<TResult>;
}
/**
 * 已分组的可枚举集合数据
 */
export declare class GroupedEnumerable<T, TKey> extends Enumerable<T> {
    /**
    * 分组的Key
    */
    key: TKey;
    constructor(groupedSource: {
        key: TKey;
        source: T[];
    });
}
/**
 * 数值型可枚举对象类
 */
export declare class NumberEnumerable extends Enumerable<number> {
    /**
     * 对可枚举对象求平均值
     */
    average(): number;
    /**
     * 返回可枚举对象中指定索引的元素，如果索引超出，返回0
     * @param index
     */
    elementAtOrDefault(index: number): number;
    /**
     * 取可枚举对象中第一个元素，如果可枚举对象元素数量为0，则返回0
     * @param predicate
     */
    firstOrDefault(predicate?: (item: number) => boolean): number;
    /**
     * 取可枚举对象中的最后一个元素，如果可枚举对象元素数量为0，则返回0
     * @param predicate
     */
    lastOrDefault(predicate?: (item: number) => boolean): number;
    /**
     * 对可枚举对象中的元素求最大值
     */
    max(): number;
    /**
     * 对可枚举对象中的元素求最小值
     */
    min(): number;
    /**
     * 对可枚举对象中元素求和
     */
    sum(): number;
}
/**
 * 已排序的可枚举数据集合
 */
export declare class OrderedEnumerable<T> extends Enumerable<T> {
    groupedSource: GroupedEnumerable<T, any>[];
    constructor(groupedSource: GroupedEnumerable<T, any>[]);
    /**
     * 对已排序的可枚举对象在保持原有排序结果的情况下再次排序（升序）
     * @param keySelector 属性选择器
     * @param comparer 属性对比器
     */
    thenBy<TKey>(keySelector: (item: T) => TKey, comparer?: (item1: TKey, item2: TKey) => number, keyEqualizer?: (item1: TKey, item2: TKey) => boolean): OrderedEnumerable<T>;
    /**
     * 对已排序的可枚举对象在保持原有排序结果的情况下再次排序（降序）
     * @param keySelector 属性选择器
     * @param comparer 属性对比器
     */
    thenByDescending<TKey>(keySelector: (item: T) => TKey, comparer?: (item1: TKey, item2: TKey) => number, keyEqualizer?: (item1: TKey, item2: TKey) => boolean): OrderedEnumerable<T>;
    private __thenBy;
}
