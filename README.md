# BuildingBlocks

一系列帮助类及扩展方法、小组件等
- #### Liyanjie.AspNetCore.Authentication.Code
    Code认证
    ```csharp
    services.AddAuthentication().AddCode("CODE");
    ```
- #### Liyanjie.AspNetCore.Extensions
    AspNetCore的一些扩展
    - DelimitedArrayAttribute
    - DelimitedArrayModelBinder
    - DelimitedArrayModelBinderProvider
    ```csharp
    IMvcBuilder.AddMvcOptions(options => 
    { 
        options.ModelBinderProviders.Insert(0, new DelimitedArrayModelBinderProvider());
    });
    class Model
    {
        [ModelBinder(typeof(Liyanjie.AspNetCore.Extensions.DelimitedArrayModelBinder))]
        [DelimitedArray(Delimiter = ",")]
        public string[] Array { get; set; }
    }
    ```
    - UniqueAuthorizationFilter
    ```csharp
    IMvcBuilder.AddMvcOptions(options =>
    {
        options.Filters.Add(new Liyanjie.AspNetCore.Extensions.UniqueAuthorizationFilter(context => string));
    });
    ```
    - WakeupBackgroundService
    ```csharp
    services.AddHostedService<WakeupBackgroundService>();  //默认从配置文件中查找键“WakeupUrl”
    ```
    - ExtendMethods
    ```csharp
    string GetClientIpAddress(this HttpContext httpContext);  //获取客户端IP地址
    bool IsValid(this ModelStateDictionary modelState, string key);  //查看模型绑定中某一项是否验证通过
    TModel BuildModel<TModel>(this QueryString queryString);  //使用QueryString构建模型
    ```
- #### Liyanjie.Drawing.Extensions
    Image绘图相关的一些扩展
    - ExtendMethods
    ```csharp
    Image Opacity(this Image image, float opacity);  //设置透明度
    Image Clear(this Image image, Color color);  //清除整个图像并以指定颜色填充
    Image Crop(this Image image, int startX, int startY, int width, int height);  //裁剪
    Image Crop(this Image image, Rectangle rectangle);  //裁剪
    Image Resize(this Image image, int? width, int? height, bool zoom = true, bool coverSize = false);  //调整尺寸
    Image Combine(this Image image, params (Point Point, Size Size, Image Image)[] images);  //组合多张图片
    Image Concatenate(this Image image, Image image2, bool direction = false);  //拼接多张图片
    void CompressSave(this Image image, string path, long quality, ImageFormat format = default);  //压缩存储
    string Encode(this Image image, ImageFormat format = default);  //将图片转换为Base64字符串
    Bitmap Decode(this string imageBase64String);  //将Base64字符串转换为图片
    ```
- #### Liyanjie.EnglishPluralization
    英语多元化
    - EnglishPluralization
    ```csharp
    string Pluralize(string word);  //将单词转为复数形式
    string Singularize(string word);  //将单词转为单数形式
    ```
- #### Liyanjie.GrpcServer
    GrpcServer实现
    ```csharp
    services.AddGrpcServer(options => 
    {
        options.AddPort(string host, int port, ServerCredentials credentials = default);
        options.AddService(serviceProvider => XXXService.BindService(new XXXServiceImpl(serviceProvider)));
    });
    ```
- #### Liyanjie.Http
    优雅的发送Http请求的方式
    - Http
    ```csharp
    Http.Do(HttpMethod method, string url)
        [.AddQuery(string name, string value)]
        [.AddHeader(string name, string value)]
        [.AddContent(HttpContent content)]
        [.UseHttpClient(HttpClient client)]
        .SendAsync();
    ```
- #### Liyanjie.Linq
    基于字符串动态构建Lambda表达式实现的Linq扩展
    - IEnumerable
    ```csharp
    object[] ToArray(this IEnumerable source);
    T[] ToArray<T>(this IEnumerable source);
    List<object> ToList(this IEnumerable source);
    List<T> ToList<T>(this IEnumerable source);
    ```
    - IQueryable
    ```csharp
    int Count(this IQueryable source, string predicate, [IDictionary<string, dynamic> parameters]);
    object FirstOrDefault(this IQueryable source, string predicate, [IDictionary<string, dynamic> parameters]);
    object LastOrDefault(this IQueryable source, string predicate, [IDictionary<string, dynamic> parameters]);
    IQueryable Where(this IQueryable source, string predicate, [IDictionary<string, dynamic> parameters]);
    object SingleOrDefault(this IQueryable source, string predicate, [IDictionary<string, dynamic> parameters]);
    // and so on…
    ```
- #### Liyanjie.Linq.Expressions
    基于字符串动态构建Lambda表达式
    - ExpressionParser
    ```csharp
    LambdaExpression ParseLambda(Type, string, IDictionary<string, object>);
    ```
- #### Liyanjie.Linq.js
    javascript的linq实现
    ```javascript
    Enumerable.new([]).orderBy(_ => _).thenByDescing(_ => _).groupBy(_ => _);
    ```
- #### Liyanjie.MongoDB.Driver.Extensions
    MongoDB Driver的一些扩展
    - MongoDBDateTimeOffsetSerializer
    ```csharp
    BsonSerializer.RegisterSerializer(new MongoDBDateTimeOffsetSerializer());
    ```
    - ExtendMethods
    ```csharp
    IMongoQueryable<TSource> IfWhere<TSource>(this IMongoQueryable<TSource> source, Func<bool> ifPredicate, Expression<Func<TSource, bool>> wherePredicate);
    ```
- #### Liyanjie.Pinyin
    基于“结巴分词”的中文获取拼音实现 
    - ExtendMethods
    ```csharp
    string[] GetPinyin(this string chineseInput);  //获取中文拼音
    string[] GetChineseWordPinyin(this string chineseWord);  //尝试获取中文词语的拼音
    string[] GetChineseCharPinyins(this char chineseChar);  //尝试获取中文字符的拼音
    ```
- #### Liyanjie.TemplateMatching
    适用于AspNet的模板匹配
    ```csharp
    var routeValues = new RouteValueDictionary();
    var templateMatcher = new TemplateMatcher(TemplateParser.Parse(string routeTemplate), routeValues);
    var isMatch = templateMatcher.TryMatch(request.Path, routeValues);
    ```
- #### Liyanjie.TypeBuilder
    动态构建Type类型
    - TypeFactory
    ```csharp
    Type CreateType(IDictionary<string, Type> properties);  //使用 属性 字典创建类型
    object CreateObject(IDictionary<string, object> values);  //使用 值 字典创建对象
    ```
- #### Liyanjie.Utility
    常用帮助类及扩展方法
    - BEncoding
    ```csharp
    object Decode(Stream stream, Encoding encoding);
    void Encode(Stream stream, Encoding encoding, object data);
    ```
    - EnumHelper
    ```csharp
    IEnumerable<T> GetItems<T>() where T : Enum;
    ```
    - ExtendMethods
    ```csharp
    //so many…
    ```
- #### Liyanjie.Utility.Cn
    中国常用的一些帮助类及扩展方法
    - ChineseCharsHelper
    ```csharp
    bool TryGetChineseCharInfo(char chineseChar, out (string Unicode, int StrokeCount, string[] Pinyins) info);
    ```
    - ExtendMethods
    ```csharp
    string ToCn(this number number, CnNumberType numberType);  //将数字转换为中文
    string ToCnNumber(this number number, bool uppercase = false);  //将数字转换为中文
    string ChangeToZHHans(this string zhHantInput);  //中文繁体转简体
    string ChangeToZHHant(this string zhHansInput);  //中文简体转繁体
    bool IsPhoneNumber_Cn(this string input);  //是否为手机号码
    string HidePhoneNumber_Cn(this string origin, char @char = '*');  //隐藏手机号码
    bool IsIdNumber_Cn(this string input);  //是否为身份证号码
    string HideIdNumber_Cn(this string origin, char @char = '*');  //隐藏身份证号码
    ```
- #### Liyanjie.Utility.js
    javascript的扩展类及扩展方法
    - Guid
    ```javascript
    Guid Guid.empty();  //00000000-0000-0000-0000-000000000000
    Guid Guid.newGuid();  //新Guid实例
    Guid.prototype.format(format?: 'N|D|B|P');  //Guid格式化，format默认为D
    ```
    - ExtendMethods
    ```javascript
    Date.prototype.format(format: string, weekDisplay: {sun,mon,tue,wed,thu,fri,sat}); //Date格式化
    Date.prototype.addMillionSeconds(millionSeconds: number);
    Date.prototype.addSeconds(seconds: number);
    Date.prototype.addMinutes(minutes: number);
    Date.prototype.addHours(hours: number);
    Date.prototype.addDays(days: number);
    Date.prototype.addMonths(months: number);
    Date.prototype.addYears(years: number);
    Number.prototype.plus(arg: number);  //加
    Number.prototype.minus(arg: number);  //减
    Number.prototype.multipy(arg: number);  //乘
    Number.prototype.divide(arg: number);  //除
    Number.prototype.toCnNumber(number: number, uppercase: boolean);  //将数字转换为中文
    Number.prototype.toCn(number: number, numberType: 'Normal|NormalUpper|Direct|DirectUpper|Currency');  //将数字转换为中文
    ```
- #### Liyanjie.ValueObjects
    常用复合类型
