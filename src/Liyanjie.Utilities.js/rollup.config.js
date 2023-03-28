const typescript = require('@rollup/plugin-typescript');

exports.default = {
    input: "./src/index.ts",
    output: {
        file: "bundles/liyanjie.utilities.umd.js",
        name: "liyanjie.utilities",
        format: "umd"
    },
    plugins: [typescript()]
};
