Date.prototype.format = function (format, weekDisplay) {
    if (weekDisplay === void 0) { weekDisplay = {}; }
    var o = {
        "M{1,2}": this.getMonth() + 1,
        "d{1,2}": this.getDate(),
        "h{1,2}": this.getHours() % 12 == 0 ? 12 : this.getHours() % 12,
        "H{1,2}": this.getHours(),
        "m{1,2}": this.getMinutes(),
        "s{1,2}": this.getSeconds(), //秒
    };
    var w = {
        "0": weekDisplay.sun || "星期日",
        "1": weekDisplay.mon || "星期一",
        "2": weekDisplay.tue || "星期二",
        "3": weekDisplay.wed || "星期三",
        "4": weekDisplay.thu || "星期四",
        "5": weekDisplay.fri || "星期五",
        "6": weekDisplay.sat || "星期六"
    };
    var match_y = format.match(/(y+)/); //年
    (match_y) && (format = format.replace(match_y[0], this.getFullYear().toString().substring(4 - match_y[0].length)));
    var match_d = format.match(/(d{3,4})/); //星期
    (match_d) && (format = format.replace(match_d[0], w[this.getDay().toString()]));
    for (var k in o) {
        var match = format.match("(".concat(k, ")"));
        if (match) {
            var value = match[0].length === 1
                ? (o[k])
                : ("00".concat(o[k])).substring(o[k].toString().length);
            format = format.replace(match[0], value);
        }
    }
    var match_f = format.match(/(f{1,3})/); //毫秒
    (match_f) && (format = format.replace(match_f[0], this.getMilliseconds().toString().substring(3 - match_f[0].length)));
    return format;
};
Date.prototype.addMillionSeconds = function (millionSeconds) {
    var date = new Date(this.getTime());
    date.setMilliseconds(date.getMilliseconds() + millionSeconds);
    return date;
};
Date.prototype.addSeconds = function (seconds) {
    var date = new Date(this.getTime());
    date.setSeconds(this.getSeconds() + seconds);
    return date;
};
Date.prototype.addMinutes = function (minutes) {
    var date = new Date(this.getTime());
    date.setMinutes(date.getMinutes() + minutes);
    return date;
};
Date.prototype.addHours = function (hours) {
    var date = new Date(this.getTime());
    date.setHours(date.getHours() + hours);
    return date;
};
Date.prototype.addDays = function (days) {
    var date = new Date(this.getTime());
    date.setDate(date.getDate() + days);
    return date;
};
Date.prototype.addMonths = function (months) {
    var date = new Date(this.getTime());
    date.setMonth(date.getMonth() + months);
    return date;
};
Date.prototype.addYears = function (years) {
    var date = new Date(this.getTime());
    date.setFullYear(date.getFullYear() + years);
    return date;
};
//# sourceMappingURL=DateExtension.js.map