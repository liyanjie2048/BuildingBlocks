(function (factory) {
	typeof define === 'function' && define.amd ? define(factory) :
	factory();
})((function () { 'use strict';

	Object.defineProperty(exports, "__esModule", { value: true });
	exports.Guid = void 0;
	require("./DateExtension");
	require("./NumberExtension");
	var Guid_1 = require("./Guid");
	Object.defineProperty(exports, "Guid", { enumerable: true, get: function () { return Guid_1.Guid; } });

}));
