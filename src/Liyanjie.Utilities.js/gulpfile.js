const gulp = require('gulp');
const rename = require('gulp-rename');
const minifyJs = require('gulp-uglify');

exports.dist = (cp) => {
    gulp.src('./bundles/liyanjie.utilities.umd.js')
        .pipe(rename('liyanjie.utilities.js'))
        .pipe(gulp.dest('./dist'))
        .pipe(rename({ suffix: '.min' }))
        .pipe(minifyJs())
        .pipe(gulp.dest('./dist'));
    cp();
};
