Number.prototype.plus = function (arg) {
    var r1, r2, m;
    try {
        r1 = this.toString().split('.')[1].length;
    }
    catch (e) {
        r1 = 0;
    }
    try {
        r2 = arg.toString().split('.')[1].length;
    }
    catch (e) {
        r2 = 0;
    }
    m = Math.pow(10, Math.max(r1, r2));
    return (this * m + arg * m) / m;
};
Number.prototype.minus = function (arg) {
    var r1, r2, m, n;
    try {
        r1 = this.toString().split('.')[1].length;
    }
    catch (e) {
        r1 = 0;
    }
    try {
        r2 = arg.toString().split('.')[1].length;
    }
    catch (e) {
        r2 = 0;
    }
    m = Math.pow(10, Math.max(r1, r2));
    n = (r1 >= r2) ? r1 : r2;
    return Number(((this * m - arg * m) / m).toFixed(n));
};
Number.prototype.multipy = function (arg) {
    var m = 0, s1 = this.toString(), s2 = arg.toString();
    try {
        m += s1.split('.')[1].length;
    }
    catch (e) { }
    try {
        m += s2.split('.')[1].length;
    }
    catch (e) { }
    return Number(s1.replace('.', '')) * Number(s2.replace('.', '')) / Math.pow(10, m);
};
Number.prototype.divide = function (arg) {
    var t1 = 0, t2 = 0, r1, r2;
    try {
        t1 = this.toString().split('.')[1].length;
    }
    catch (e) { }
    try {
        t2 = arg.toString().split('.')[1].length;
    }
    catch (e) { }
    r1 = Number(this.toString().replace('.', ''));
    r2 = Number(arg.toString().replace('.', ''));
    return (r1 / r2) * Math.pow(10, t2 - t1);
};
function __toCnNumber(number, uppercase) {
    switch (number.toString()) {
        case '0':
            return uppercase ? '零' : '〇';
        case '1':
            return uppercase ? '壹' : '一';
        case '2':
            return uppercase ? '贰' : '二';
        case '3':
            return uppercase ? '叁' : '三';
        case '4':
            return uppercase ? '肆' : '四';
        case '5':
            return uppercase ? '伍' : '五';
        case '6':
            return uppercase ? '陆' : '六';
        case '7':
            return uppercase ? '柒' : '七';
        case '8':
            return uppercase ? '捌' : '八';
        case '9':
            return uppercase ? '玖' : '九';
        case '10':
            return uppercase ? '拾' : '十';
        case '100':
            return uppercase ? '佰' : '百';
        case '1000':
            return uppercase ? '仟' : '千';
        case '10000':
            return '万';
        case '100000000':
            return '亿';
        case '1000000000000':
            return '兆';
        case '10000000000000000':
            return '京';
        case '100000000000000000000':
            return '垓';
        case '1e+24':
            return '秭';
        case '1e+28':
            return '穰';
        case '1e+32':
            return '沟';
        case '9.999999999999999e+35':
            return '涧';
        case '1e+40':
            return '正';
        case '1e+44':
            return '载';
        case '1e+48':
            return '极';
        default:
            break;
    }
}
;
Number.prototype.toCn = function (outputType) {
    if (outputType === void 0) { outputType = 'number'; }
    outputType = outputType.toLowerCase();
    var number = this;
    var numberString = this.toString();
    var s = '';
    if (numberString[0] == '-') {
        s += '负';
        number = Math.abs(number);
    }
    var unit = function (i, currency) {
        return currency
            ? i < 3 ? '角分厘'[i] : ''
            : i < 7 ? '分釐毫丝忽微纤'[i] : '';
    }, processLong = function (number, currency) {
        var sb = '';
        var zeroFlag = false; //输出0
        if (number > 1) {
            var l = number;
            for (var i = 12; i >= 0; i--) {
                var level = Math.pow(10000, i);
                //console.log('level=' + level);
                if (number >= level) {
                    l = number % level;
                    number = Math.trunc(number / level);
                    //console.log('l=' + l + ',number=' + number);
                    if (number > 19) {
                        var j = 1000;
                        while (number % (j * 10) >= 1) {
                            var tmp = Math.trunc(number / j);
                            //console.log(',tmp=' + number);
                            if (tmp != 0) {
                                sb += __toCnNumber(tmp, currency);
                                if (j > 1)
                                    sb += __toCnNumber(j, currency);
                                zeroFlag = true;
                            }
                            else if (zeroFlag) {
                                sb += __toCnNumber(0, currency);
                                zeroFlag = false;
                            }
                            if (j == 1)
                                break;
                            number %= j;
                            j = j / 10;
                        }
                    }
                    else if (number >= 10) {
                        sb += __toCnNumber(10, currency);
                        if (number % 10 > 0) {
                            sb += __toCnNumber(number % 10, currency);
                            zeroFlag = true;
                        }
                    }
                    else
                        sb += __toCnNumber(number, currency);
                    if (level > 1)
                        sb += __toCnNumber(level, currency);
                }
                number = l;
            }
        }
        else
            sb += __toCnNumber(number, currency);
        return sb;
    }, processDecimal = function (decimal, currency) {
        var sb = '';
        var zeroFlag = false; //输出0
        for (var i = 0; i < decimal.length; i++) {
            var d = parseInt(decimal[i]);
            if (d > 0) {
                sb += __toCnNumber(d, currency);
                sb += unit(i, currency);
                zeroFlag = true;
            }
            else if (decimal.length > i + 1 && zeroFlag) {
                sb += __toCnNumber(0, currency);
                zeroFlag = false;
            }
        }
        return sb;
    };
    var int = Math.trunc(number);
    switch (outputType) {
        case 'number':
            s += processLong(int, false);
            break;
        case 'currency':
            s += processLong(int, true);
            break;
        case 'digit':
            var s_ = int.toString();
            for (var i = 0; i < s_.length; i++) {
                s += __toCnNumber(parseInt(s_[i]), false);
            }
            break;
        default:
            break;
    }
    var dec, index = numberString.indexOf('.');
    if (index > 0)
        dec = numberString.substr(index + 1);
    if (dec) {
        switch (outputType) {
            case 'number':
                s += '又';
                break;
            case 'currency':
                s += '圆';
                break;
            case 'digit':
                s += '点';
                break;
            default:
                break;
        }
        switch (outputType) {
            case 'number':
                s += processDecimal(dec, false);
                break;
            case 'currency':
                s += processDecimal(dec, true);
                break;
            case 'digit':
                for (var i = 0; i < dec.length; i++) {
                    s += __toCnNumber(parseInt(dec[i]), false);
                }
                break;
            default:
                break;
        }
    }
    else if (outputType == 'currency')
        s += '圆';
    return s;
};
//# sourceMappingURL=NumberExtension.js.map