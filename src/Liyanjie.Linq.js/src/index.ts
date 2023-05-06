/**
 * 可枚举对象类
 */
export class Enumerable<T> {

    /**
    *源数组，所有的操作基于此
    */
    source: T[];

    /**
     * 创建一个Enumerable对象，基于此对象可以使用LinqJs的各种方法
     * @param source 源数组
     */
    constructor(source: T[])
    {
        Enumerable._check(source);
        this.source = source;
    }

    private static _check<T>(array: T[]): void
    {
        if (array === null || array === undefined || !Array.isArray(array))
            throw new Error('Array parameter can not be null or undefined!');
    }

    /**
     * 创建一个Enumerable对象，基于此对象可以使用LinqJs的各种方法
     * @param source 源数组
     * @returns 
     */
    static new<TSource>(source?: TSource[]): Enumerable<TSource>
    {
        return new Enumerable(source || []);
    }

    /**
     * 创建一个Enumerable对象，基于此对象可以使用LinqJs的各种方法
     * @param source 源数组
     */
    static from<TSource>(source?: TSource[]): Enumerable<TSource>
    {
        return new Enumerable(source || []);
    }

    /**
     * 判断可枚举对象中的每一元素能够满足指定条件
     * @param predicate 条件表达式
     */
    all(predicate: (item: T) => boolean): boolean
    {
        return this.every(predicate);
    }

    /**
     * 判断可枚举对象中的每一元素能够满足指定条件
     * @param predicate 条件表达式
     */
    every(predicate: (item: T) => boolean): boolean
    {
        return this.source.every(predicate) === true;
    }

    /**
     * 判断可枚举对象中的任一对象能够满足指定条件
     * @param predicate
     */
    any(predicate?: (item: T) => boolean): boolean
    {
        return this.some(predicate);
    }

    /**
     * 判断可枚举对象中的任一对象能够满足指定条件
     * @param predicate
     */
    some(predicate?: (item: T) => boolean): boolean
    {
        return predicate
            ? this.source.some(predicate) === true
            : this.source.length > 0;
    }

    /**
     * 向可枚举对象的末尾追加元素
     * @param elements 元素
     */
    append(...elements: T[]): Enumerable<T>
    {
        Enumerable._check(elements);
        this.source.push(...elements);
        return this;
    }

    /**
     * 对可枚举对象中的元素的指定属性求平均值
     * @param selector 属性表达式
     */
    average(selector: (item: T) => number): number
    {
        return this.sum(selector) / this.source.length;
    }

    /**
     * 拼接序列
     * @param targets 目标
     */
    concat(...targets: T[][]): Enumerable<T>
    {
        Enumerable._check(targets);
        return new Enumerable(this.source.concat(...targets));
    }

    /**
     * 判断可枚举对象中是否包含指定元素
     * @param element 指定元素
     * @param comparer 元素对比器
     */
    contains(element: T, comparer?: (item1: T, item2: T) => boolean): boolean
    {
        return comparer
            ? this.source.some(_ => comparer(_, element)) === true
            : this.source.indexOf(element) > -1;
    }

    /**
     * 取可枚举对象中元素的数量
     * @param predicate 条件表达式
     */
    count(predicate?: (item: T) => boolean): number
    {
        return predicate
            ? this.source.filter(predicate).length
            : this.source.length;
    }

    /**
     * 如果可枚举对象中元素数量为0，则返回包含默认值的新可枚举对象
     * @param defaultValue 默认值
     */
    defaultIfEmpty(defaultValue?: T): Enumerable<T>
    {
        return this.isEmpty()
            ? new Enumerable(defaultValue ? [defaultValue] : [])
            : this;
    }

    /**
     * 对可枚举对象中的元素去重
     * @param comparer 元素对比器
     */
    distinct(comparer?: (item1: T, item2: T) => boolean): Enumerable<T>
    {
        comparer = comparer || ((item1, item2) => item1 === item2);
        let result: T[] = [];
        this.source.forEach(item => (result.some(_ => comparer(_, item)) === false) && result.push(item));
        return new Enumerable(result);
    }

    /**
     * 返回可枚举对象中指定索引的元素，如果索引超出，返回undefined
     * @param index
     */
    elementAtOrDefault(index: number): T | undefined
    {
        return this.source.length > index
            ? this.source[index]
            : undefined;
    }

    /**
     * 生成一个空的可枚举对象
     */
    static empty<TSource>(): Enumerable<TSource>
    {
        return new Enumerable([]);
    }

    /**
     * 排除可枚举对象中存在于目标序列的元素
     * @param target 目标序列
     * @param comparer 元素对比器
     */
    except(target: T[], comparer?: (item1: T, item2: T) => boolean): Enumerable<T>
    {
        Enumerable._check(target);
        comparer = comparer || ((item1, item2) => item1 === item2);
        let result: T[] = [];
        this.source.forEach(item => (target.some(_ => comparer(_, item)) === false) && result.push(item));
        return new Enumerable(result);
    }

    /**
     * 取可枚举对象中满足条件的第一个元素，如果可枚举对象元素数量为0，则返回undefined
     * @param predicate 条件表达式
     */
    firstOrDefault(predicate?: (item: T) => boolean): T | undefined
    {
        let source = predicate
            ? this.source.filter(predicate)
            : this.source;
        return source.length > 0
            ? source[0]
            : undefined;
    }

    /**
     * ForEach循环
     * @param callbackFn 回掉函数
     */
    forEach(callbackFn: (value: T, index: number) => void): void
    {
        this.source.forEach(callbackFn);
    }

    /**
     * 对可枚举对象中的元素进行分组
     * @param keySelector 属性选择器，最为分组依据
     * @param comparer 属性对比器
     */
    groupBy<TKey>(
        keySelector: (item: T) => TKey,
        comparer?: (item1: TKey, item2: TKey) => boolean): Enumerable<GroupedEnumerable<T, TKey>>
    {
        comparer = comparer || ((item1, item2) => item1 === item2);
        let result: GroupedEnumerable<T, TKey>[] = [];
        this.source.forEach(item =>
        {
            let key = keySelector(item);
            let array = result.filter(_ => comparer(_.key, key));
            array.length > 0
                ? array[0].source.push(item)
                : result.push(new GroupedEnumerable({ key: key, source: [item] }));
        });
        return new Enumerable(result);
    }

    /**
     * 将可枚举对象与目标序列进行融合
     * @param target 目标序列
     * @param keySelector 属性选择器
     * @param targetKeySelector 属性选择器
     * @param resultSelector 结果选择器
     * @param comparer 属性对比器
     */
    groupJoin<TTarget, TKey, TResult>(
        target: TTarget[],
        keySelector: (item: T) => TKey,
        targetKeySelector: (item: TTarget) => TKey,
        resultSelector: (item1: T, item2: TTarget[], key?: TKey) => TResult,
        comparer?: (item1: TKey, item2: TKey) => boolean): Enumerable<TResult>
    {
        Enumerable._check(target);
        comparer = comparer || ((item1, item2) => item1 === item2);
        let result: TResult[] = [];
        for (let i = 0; i < this.source.length; i++)
        {
            let item1 = this.source[i];
            let key = keySelector(item1);
            let item2 = target.filter(_ => comparer(key, targetKeySelector(_)));
            let selected = resultSelector(item1, item2, key);
            selected && result.push(selected);
        }
        return new Enumerable(result);
    }

    /**
     * 取可枚举对象中与目标序列的交集
     * @param target 目标序列
     * @param comparer 元素对比器
     */
    intersect(
        target: T[],
        comparer?: (item1: T, item2: T) => boolean): Enumerable<T>
    {
        Enumerable._check(target);
        comparer = comparer || ((item1, item2) => item1 === item2);
        let result: T[] = [];
        this.source.forEach(item => (target.some(_ => comparer(_, item)) === true) && result.push(item));
        return new Enumerable(result);
    }

    /**取可枚举对象中的第一个元素，如果可枚举对象元素数量为0，则返回undefined
     * 判断可枚举对象是否包含元素
     */
    isEmpty(): boolean
    {
        return this.source.length === 0;
    }

    /**
     * 以相等属性为条件将可枚举对象与目标序列融合
     * @param target 目标序列
     * @param keySelector 属性选择器
     * @param targetKeySelector 属性选择器
     * @param resultSelector 结果选择器
     * @param comparer 属性对比器
     */
    join<TTarget, TKey, TResult>(
        target: TTarget[],
        keySelector: (item: T) => TKey,
        targetKeySelector: (item: TTarget) => TKey,
        resultSelector: (item1: T, item2?: TTarget, key?: TKey) => TResult,
        comparer?: (item1: TKey, item2: TKey) => boolean): Enumerable<TResult>
    {
        Enumerable._check(target);
        comparer = comparer || ((item1, item2) => item1 === item2);
        let result: TResult[] = [];
        for (let i = 0; i < this.source.length; i++)
        {
            let item1 = this.source[i];
            let key = keySelector(item1);
            let filteredTarget = target.filter(_ => comparer(key, targetKeySelector(_)));
            let item2 = filteredTarget.length > 0
                ? filteredTarget[0]
                : undefined;
            let selected = resultSelector(item1, item2, key);
            selected && result.push(selected);
        }
        return new Enumerable(result);
    }

    /**
     * 取可枚举对象中满足条件的最后一个元素，如果可枚举对象元素数量为0，则返回undefined
     * @param predicate 条件表达式
     */
    lastOrDefault(predicate?: (item: T) => boolean): T | undefined
    {
        let source = predicate
            ? this.source.filter(predicate)
            : this.source;
        return this.source.length > 0
            ? this.source[this.source.length - 1]
            : undefined;
    }

    /**
     * 对可枚举对象中的元素的指定属性求最大值
     * @param selector 属性表达式
     */
    max(selector: (item: T) => number): number
    {
        return this.source.length === 0
            ? 0
            : selector(this.source.sort((item1, item2) => selector(item2) - selector(item1))[0]);
    }

    /**
     * 对可枚举对象中的元素的指定属性求最小值
     * @param selector 属性表达式
     */
    min(selector: (item: T) => number): number
    {
        return this.source.length === 0
            ? 0
            : selector(this.source.sort((item1, item2) => selector(item1) - selector(item2))[0]);
    }

    /**
     * 对可枚举对象中的元素进行排序（升序）
     * @param keySelector 属性选择器
     * @param comparer 属性对比器
     */
    orderBy<TKey>(
        keySelector: (item: T) => TKey,
        comparer?: (item1: TKey, item2: TKey) => number,
        keyEqualizer?: (item1: TKey, item2: TKey) => boolean): OrderedEnumerable<T>
    {
        return this.__orderBy(keySelector, false, comparer, keyEqualizer);
    }

    /**
     * 对可枚举对象中的元素进行排序（降序）
     * @param keySelector
     * @param comparer
     */
    orderByDescending<TKey>(
        keySelector: (item: T) => TKey,
        comparer?: (item1: TKey, item2: TKey) => number,
        keyEqualizer?: (item1: TKey, item2: TKey) => boolean): OrderedEnumerable<T>
    {
        return this.__orderBy(keySelector, true, comparer, keyEqualizer);
    }

    private __orderBy<TKey>(
        keySelector: (item: T) => TKey,
        descending: boolean,
        comparer?: (item1: TKey, item2: TKey) => number,
        keyEqualizer?: (item1: TKey, item2: TKey) => boolean): OrderedEnumerable<T>
    {
        keyEqualizer = keyEqualizer || ((item1, item2) => item1 === item2);
        let keys: TKey[] = [];
        let group: { key: TKey, source: T[]; }[] = [];
        this.source.forEach(item =>
        {
            const key = keySelector(item);
            keys.indexOf(key) < 0 && keys.push(key);
            const array = group.filter(_ => keyEqualizer(_.key, key));
            array.length > 0
                ? array[0].source.push(item)
                : group.push({ key: key, source: [item] });
        });
        keys = comparer ? keys.sort(comparer) : keys.sort();
        (descending) && (keys = keys.reverse());
        let result: GroupedEnumerable<T, any>[] = [];
        keys.forEach(item => result.push(new GroupedEnumerable(group.filter(_ => keyEqualizer(item, _.key))[0])));
        keys = undefined;
        group = undefined;
        return new OrderedEnumerable(result);
    }

    /**
     * 向可枚举对象的开头添加元素
     * @param elements 目标元素
     */
    prepend(...elements: T[]): Enumerable<T>
    {
        Enumerable._check(elements);
        this.source = new Array(...elements, ...this.source);
        return this;
    }

    /**
     * 生成一个新的数值型可枚举对象
     * @param start 起始值
     * @param count 元素量
     */
    static range(start: number, count: number): NumberEnumerable
    {
        let result: number[] = [];
        for (let i = 0; i < count; i++)
        {
            result.push(start + i);
        }
        return new NumberEnumerable(result);
    }

    /**
     * 向可枚举对象中重复添加元素
     * @param element 目标元素
     * @param count 添加数量
     */
    static repeat<TSource>(element: TSource, count: number): Enumerable<TSource>
    {
        let result: TSource[] = [];
        for (let i = 0; i < count; i++)
        {
            result.push(element);
        }
        return new Enumerable(result);
    }

    /**
     * 对可枚举对象中的元素反向排序
     */
    reverse(): Enumerable<T>
    {
        return new Enumerable(new Array(...this.source).reverse());
    }

    /**
     * 遍历可枚举对象并生成一个新的可枚举对象
     * @param selector 元素选择器
     */
    select<TResult>(selector: (item: T, index?: number) => TResult): Enumerable<TResult>
    {
        let result: TResult[] = [];
        this.source.forEach((item, index) =>
        {
            let selected = selector(item, index);
            selected && result.push(selected);
        });
        return new Enumerable(result);
    }

    /**
     * 遍历可枚举对象并生成一个新的可枚举对象
     * @param resultSelector 序列选择器
     */
    selectMany<TResult>(resultSelector: (item: T, index?: number) => TResult[]): Enumerable<TResult>
    {
        let result: TResult[] = [];
        this.source.forEach((item, index) =>
        {
            let selected = resultSelector(item, index);
            selected && result.push(...selected);
        });
        return new Enumerable(result);
    }

    /**
     * 对比可枚举对象与目标序列中的每一个元素
     * @param target 目标序列
     * @param comparer 元素对比器
     */
    sequenceEqual(
        target: T[],
        comparer?: (item1: T, item2?: T) => boolean): boolean
    {
        Enumerable._check(target);
        if (this.source.length !== target.length)
            return false;
        comparer = comparer || ((item1, item2) => item1 === item2);
        let result: boolean = true;
        for (let i = 0; i < this.source.length; i++)
        {
            let item1 = this.source[i];
            let item2 = target.length > i ? target[i] : undefined;
            if (!comparer(item1, item2))
            {
                result = false;
                break;
            }
        }
        comparer = undefined;
        return result;
    }

    /**
     * 跳过指定数量的元素取剩下的元素
     * @param count 数量
     */
    skip(count: number): Enumerable<T>
    {
        let result: T[] = [];
        this.source.forEach((item, index) => (index >= count) && result.push(item));
        return new Enumerable(result);
    }

    /**
     * 跳过满足条件的元素取第一组连续元素
     * @param predicate 条件表达式
     */
    skipWhile(predicate: (item: T, index?: number) => boolean)
    {
        let result: T[] = [];
        let flag = false;
        for (let i = 0; i < this.source.length; i++)
        {
            let item = this.source[i];
            if (!predicate(item, i))
            {
                (!flag) && (flag = true);
                result.push(item);
            } else if (flag)
                break;
        }
        this.source.forEach((item, index) => (!predicate(item, index)) && result.push(item));
        return new Enumerable(result);
    }

    /**
     * 对可枚举对象中元素的属性求和
     * @param selector 属性选择器
     */
    sum(selector: (item: T) => number): number
    {
        if (this.source.length === 0)
            return 0;
        let sum = 0;
        this.source.forEach(item => sum += selector(item));
        return sum;
    }

    /**
     * 取可枚举对象中指定数量的元素
     * @param count 数量
     */
    take(count: number): Enumerable<T>
    {
        let result: T[] = [];
        this.source.forEach((item, index) => (index < count) && result.push(item));
        return new Enumerable(result);
    }

    /**
     * 取可枚举对象满足条件的第一组连续元素
     * @param predicate
     */
    takeWhile(predicate: (item: T, index?: number) => boolean): Enumerable<T>
    {
        let result: T[] = [];
        let flag: boolean = false;
        for (let i = 0; i < this.source.length; i++)
        {
            let item = this.source[i];
            if (predicate(item, i))
            {
                (!flag) && (flag = true);
                result.push(item);
            } else if (flag)
                break;
        }
        return new Enumerable(result);
    }

    /**
     * 将可枚举对象中的元素输出为数组
     */
    toArray(): T[]
    {
        return this.source;
    }

    /**
     * 将可枚举对象转换为键值对可枚举对象
     * @param keySelector 属性选择器
     * @param valueSelector 属性选择器
     */
    toDictionary<TValue>(keySelector: (item: T) => string, valueSelector: (item: T) => TValue): { [key: string]: TValue; }
    {
        let result: { [key: string]: TValue; } = {};
        this.source.forEach(item =>
        {
            let key = keySelector(item);
            (!result[key]) && (result[key] = valueSelector(item));
        });
        return result;
    }

    /**
     * 将可枚举对象中的元素转换为字符串并拼接
     * @param separator 拼接符
     * @param stringSelector 字符串选择器
     */
    toString(separator?: string, stringSelector?: (item: T) => string): string
    {
        (separator === null || separator === undefined) && (separator = ',');
        stringSelector = stringSelector || (item => item.toString());
        let result: string = '';
        this.source.forEach(item => result += stringSelector(item) + separator);
        return result.substring(0, result.length - separator.length);
    }

    /**
     * 连接可枚举对象与目标序列，并排除目标序列在当前可枚举对象中已存在的元素
     * @param target
     * @param comparer
     */
    union(target: T[], comparer?: (item1: T, item2: T) => boolean): Enumerable<T>
    {
        Enumerable._check(target);
        comparer = comparer || ((item1, item2) => item1 === item2);
        let result: T[] = new Array(...this.source);
        target.forEach(item => (result.some(item2 => comparer(item, item2)) === false) && result.push(item));
        comparer = undefined;
        return new Enumerable(result);
    }

    /**
     * 取可枚举对象中满足条件的元素
     * @param predicate
     */
    where(predicate: (item: T, index?: number) => boolean): Enumerable<T>
    {
        let result: T[] = [];
        this.source.forEach((item, index) => (predicate(item, index)) && result.push(item));
        return new Enumerable(result);
    }

    /**
     * 以索引位置为条件将可枚举对象与目标序列融合
     * @param target
     * @param resultSelector
     */
    zip<TTarget, TResult>(
        target: TTarget[],
        resultSelector: (item: T, item2?: TTarget, index?: number) => TResult): Enumerable<TResult>
    {
        Enumerable._check(target);
        let result: TResult[] = [];
        for (let i = 0; i < this.source.length; i++)
        {
            let item1 = this.source[i];
            let item2 = target.length > i
                ? target[i]
                : undefined;
            result.push(resultSelector(item1, item2, i));
        }
        return new Enumerable(result);
    }
}

/**
 * 已分组的可枚举集合数据
 */
export class GroupedEnumerable<T, TKey> extends Enumerable<T> {
    /**
    * 分组的Key
    */
    key: TKey;
    constructor(groupedSource: { key: TKey, source: T[]; })
    {
        super(groupedSource.source);
        this.key = groupedSource.key;
    }
}

/**
 * 数值型可枚举对象类
 */
export class NumberEnumerable extends Enumerable<number> {
    /**
     * 对可枚举对象求平均值
     */
    average(): number
    {
        return this.sum() / this.source.length;
    }

    /**
     * 返回可枚举对象中指定索引的元素，如果索引超出，返回0
     * @param index
     */
    elementAtOrDefault(index: number): number
    {
        return this.source.length > index
            ? this.source[index]
            : 0;
    }

    /**
     * 取可枚举对象中第一个元素，如果可枚举对象元素数量为0，则返回0
     * @param predicate
     */
    firstOrDefault(predicate?: (item: number) => boolean): number
    {
        return this.source.length > 0
            ? this.source[0]
            : 0;
    }

    /**
     * 取可枚举对象中的最后一个元素，如果可枚举对象元素数量为0，则返回0
     * @param predicate
     */
    lastOrDefault(predicate?: (item: number) => boolean): number
    {
        return this.source.length > 0
            ? this.source[this.source.length - 1]
            : 0;
    }

    /**
     * 对可枚举对象中的元素求最大值
     */
    max(): number
    {
        return this.source.length === 0
            ? 0
            : this.source.sort((item1, item2) => item2 - item1)[0];
    }

    /**
     * 对可枚举对象中的元素求最小值
     */
    min(): number
    {
        return this.source.length === 0
            ? 0
            : this.source.sort((item1, item2) => item1 - item2)[0];
    }

    /**
     * 对可枚举对象中元素求和
     */
    sum(): number
    {
        if (this.source.length === 0)
            return 0;
        let sum: number = 0;
        this.source.forEach(item => sum += item);
        return sum;
    }
}

/**
 * 已排序的可枚举数据集合
 */
export class OrderedEnumerable<T> extends Enumerable<T> {
    groupedSource: GroupedEnumerable<T, any>[];

    constructor(groupedSource: GroupedEnumerable<T, any>[])
    {
        super(Enumerable.new(groupedSource).selectMany(item => item.source).toArray());
        this.groupedSource = groupedSource;
    }

    /**
     * 对已排序的可枚举对象在保持原有排序结果的情况下再次排序（升序）
     * @param keySelector 属性选择器
     * @param comparer 属性对比器
     */
    thenBy<TKey>(keySelector: (item: T) => TKey, comparer?: (item1: TKey, item2: TKey) => number, keyEqualizer?: (item1: TKey, item2: TKey) => boolean): OrderedEnumerable<T>
    {
        return this.__thenBy(keySelector, false, comparer, keyEqualizer);
    }

    /**
     * 对已排序的可枚举对象在保持原有排序结果的情况下再次排序（降序）
     * @param keySelector 属性选择器
     * @param comparer 属性对比器
     */
    thenByDescending<TKey>(keySelector: (item: T) => TKey, comparer?: (item1: TKey, item2: TKey) => number, keyEqualizer?: (item1: TKey, item2: TKey) => boolean): OrderedEnumerable<T>
    {
        return this.__thenBy(keySelector, true, comparer, keyEqualizer);
    }

    private __thenBy<TKey>(keySelector: (item: T) => TKey, descending: boolean, comparer?: (item1: TKey, item2: TKey) => number, keyEqualizer?: (item1: TKey, item2: TKey) => boolean): OrderedEnumerable<T>
    {
        keyEqualizer = keyEqualizer || ((item1, item2) => item1 === item2);
        let result: GroupedEnumerable<T, any>[] = [];
        this.groupedSource.forEach(item =>
        {
            let enumerable = descending
                ? item.orderByDescending(keySelector, comparer, keyEqualizer)
                : item.orderBy(keySelector, comparer, keyEqualizer);
            enumerable.groupedSource.forEach(item2 => result.push(new GroupedEnumerable({ key: item.key + ':' + item2.key, source: item2.source })));
        });
        return new OrderedEnumerable(result);
    }
}
