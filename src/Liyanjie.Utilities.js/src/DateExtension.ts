﻿interface Date {
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

Date.prototype.format = function (format?: string, weekDisplay: WeekDisplay = {}) {
    let o = {
        "M{1,2}": (<Date>this).getMonth() + 1, //月份
        "d{1,2}": (<Date>this).getDate(), //日
        "h{1,2}": (<Date>this).getHours() % 12 == 0 ? 12 : (<Date>this).getHours() % 12, //小时
        "H{1,2}": (<Date>this).getHours(), //小时
        "m{1,2}": (<Date>this).getMinutes(), //分
        "s{1,2}": (<Date>this).getSeconds(), //秒
    };
    let w = {
        "0": weekDisplay.sun || "星期日",
        "1": weekDisplay.mon || "星期一",
        "2": weekDisplay.tue || "星期二",
        "3": weekDisplay.wed || "星期三",
        "4": weekDisplay.thu || "星期四",
        "5": weekDisplay.fri || "星期五",
        "6": weekDisplay.sat || "星期六"
    };
    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (<Date>this).getFullYear().toString().substr(4 - RegExp.$1.length));
    }
    if (/(d{3,4})/.test(format)) {
        format = format.replace(RegExp.$1, w[(<Date>this).getDay().toString()]);
    }
    for (let k in o) {
        if (new RegExp(`(${k})`).test(format)) {
            let value = RegExp.$1.length === 1
                ? (o[k])
                : (`00${o[k]}`).substr(o[k].toString().length);
            format = format.replace(RegExp.$1, value);
        }
    }
    if (/(f{1,3})/.test(format)) {
        format = format.replace(RegExp.$1, (<Date>this).getMilliseconds().toString().substr(3 - RegExp.$1.length));
    }
    return format;
};
Date.prototype.addMillionSeconds = function (millionSeconds: number) {
    let date = new Date((<Date>this).getTime());
    date.setMilliseconds(date.getMilliseconds() + millionSeconds);
    return date;
};
Date.prototype.addSeconds = function (seconds: number) {
    let date = new Date((<Date>this).getTime());
    date.setSeconds((<Date>this).getSeconds() + seconds);
    return date;
};
Date.prototype.addMinutes = function (minutes: number) {
    let date = new Date((<Date>this).getTime());
    date.setMinutes(date.getMinutes() + minutes);
    return date;
};
Date.prototype.addHours = function (hours: number) {
    let date = new Date((<Date>this).getTime());
    date.setHours(date.getHours() + hours);
    return date;
};
Date.prototype.addDays = function (days: number) {
    let date = new Date((<Date>this).getTime());
    date.setDate(date.getDate() + days);
    return date;
};
Date.prototype.addMonths = function (months: number) {
    let date = new Date((<Date>this).getTime());
    date.setMonth(date.getMonth() + months);
    return date;
};
Date.prototype.addYears = function (years: number) {
    let date = new Date((<Date>this).getTime());
    date.setFullYear(date.getFullYear() + years);
    return date;
};
