# BuildingBlocks

一系列帮助类及扩展方法、小组件等

- #### Liyanjie.AdministrativeDevision.Cn
    省市县三级数据
  - ADCnHelper
    ```csharp
    // when code="0",return all provinces.
    bool TryGetChildren(string code, out Dictionary<string, string>? children);
    // result may be like ["省", "市", "县"]
    string[]? Display(string code);
    ```
- #### Liyanjie.AspNetCore.Authentication.Code
    Code认证
  - Extension Methods
    ```csharp
    // services is IServiceCollection
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
        public string[] ArrayProperty { get; set; }
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
  - Extension Methods
    ```csharp
    string GetClientIpAddress(this HttpContext httpContext);  //获取客户端IP地址
    bool IsValid(this ModelStateDictionary modelState, string key);  //查看模型绑定中某一项是否验证通过
    TModel BuildModel<TModel>(this QueryString queryString);  //使用QueryString构建模型
    ```
- #### Liyanjie.AspNetCore.Hosting.WindowsDesktop
    可以在Windows桌面上托管AspNetCore应用
  - 新建 net6.0 的 AspNetCore 应用
  - 安装 Liyanjie.AspNetCore.Hosting.WindowsDesktop.1.0.0.nupkg (https://www.myget.org/feed/liyanjie/package/nuget/Liyanjie.AspNetCore.Hosting.WindowsDesktop)
  - 将 Host 目录中对应版本的文件夹下的所有文件设置 “生成操作=内容”、“复制到输出目录=始终复制”
  - 编译 AspNetCore 应用
  - 发布 AspNetCore 应用
  - 进入发布后的目录，将 Host\对应版本\* 目录下所有文件移动到与 AspNetCore 应用同一目录
  - 配置 Host.exe.config，将 Startup 指向 AspNetCore 应用的 dll
  - 运行 Host.exe
  - 在 Host.exe.config 中可以配置桌面应用图标与名称、绑定 host 和 port
- #### Liyanjie.Drawing.Extensions
    Image绘图相关的一些扩展
  - GifWriter
    ```csharp
    void WriteFrame(Image image, int delay = 0);
    ```
  - ImageX
    ```csharp
    Image? FromBase64String(string imageBase64String);
    Image? FromDataUrl(string imageDataUrl);
    ```
  - Extension Methods
    ```csharp
    Image SetOpacity(this Image image, float opacity);  //设置透明度
    Image Clear(this Image image, Color color);  //清除整个图像并以指定颜色填充
    Image Crop(this Image image, int startX, int startY, int width, int height);  //裁剪
    Image Crop(this Image image, Rectangle rectangle);  //裁剪
    Image Resize(this Image image, int? width, int? height, bool zoom = true, bool coverSize = false);  //调整尺寸
    Image Combine(this Image image, params (Point Point, Size Size, Image Image)[] images);  //组合多张图片
    Image CombineToGif(this Image image, ushort delay = 0, int repeat = 0, params (Point Point, Size Size, Image Image, int Delay)[] images);  //组合多张图片并生成GIF
    Image Concatenate(this Image image, Image image2, bool direction = false);  //拼接多张图片
    void CompressSave(this Image image, string path, long quality, ImageFormat format = default);  //压缩存储
    string ToBase64String(this Image image, ImageFormat format = default);  //将图片转换为Base64字符串
    string ToDataUrl(this Image image, ImageFormat format = default);  //将图片转换为DataUri字符串
    Bitmap Decode(this string imageBase64String);  //将Base64字符串转换为图片
    ```
- #### Liyanjie.Drawing.ImageSharp
    Image绘图相关的一些扩展
  - ImageSharpX
    ```csharp
    void WriteFrame(Image image, int delay = 0);
    Image? FromBase64String(string imageBase64String);
    Image? FromDataUrl(string imageDataUrl);
    Image Load(string path);
    Image LoadAsync(string path);
    ```
    ```
  - Extension Methods
    ```csharp
    Image SetOpacity(this Image image, float opacity);  //设置透明度
    Image Clear(this Image image, Color color);  //清除整个图像并以指定颜色填充
    Image Crop(this Image image, int startX, int startY, int width, int height);  //裁剪
    Image Crop(this Image image, Rectangle rectangle);  //裁剪
    Image Resize(this Image image, int? width, int? height, bool zoom = true, bool coverSize = false);  //调整尺寸
    Image Combine(this Image image, params (Point Point, Size Size, Image Image)[] images);  //组合多张图片
    Image CombineToGif(this Image image, ushort delay = 0, int repeat = 0, params (Point Point, Size Size, Image Image, int Delay)[] images);  //组合多张图片并生成GIF
    Image Concatenate(this Image image, Image image2, bool direction = false);  //拼接多张图片
    string ToBase64String(this Image image, IImageFormat format = default);  //将图片转换为Base64字符串
    string ToDataUrl(this Image image, IImageFormat format = default);  //将图片转换为DataUri字符串
    ```
- #### Liyanjie.Drawing.SkiaSharp
    Image绘图相关的一些扩展
  - SKImageX
    ```csharp
    SKImage FromFile(string filePath);
    SKImage? FromBase64String(string imageBase64String);
    SKImage? FromDataUrl(string imageDataUrl);
    ```
    ```
  - Extension Methods
    ```csharp
    SKImage SetOpacity(this SKImage image, float opacity);  //设置透明度
    SKImage Clear(this SKImage image, SKColor color);  //清除整个图像并以指定颜色填充
    SKImage Crop(this SKImage image, int startX, int startY, int width, int height);  //裁剪
    SKImage Crop(this SKImage image, SKRect rect);  //裁剪
    SKImage Resize(this SKImage image, int? width, int? height, SKFilterQuality quality = SKFilterQuality.None, bool zoom = true, bool coverSize = false);  //调整尺寸
    SKImage Combine(this SKImage image, params (SKPoint Point, SKSize Size, SKImage Image)[] images);  //组合多张图片
    SKImage Concatenate(this SKImage image, SKImage image2, bool direction = false);  //拼接多张图片
    void Save(this SKImage image, string path, (SKEncodedImageFormat Format, int Quality)? format = default);  //存储
    Task SaveAsync(this SKImage image, string path, (SKEncodedImageFormat Format, int Quality)? format = default);  //存储
    string ToBase64String(this SKImage image, (SKEncodedImageFormat Format, int Quality)? format = default);  //将图片转换为Base64字符串
    string ToDataUrl(this SKImage image, (SKEncodedImageFormat Format, int Quality)? format = default);  //将图片转换为DataUri字符串
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
  - Extension Methods
    ```csharp
    // services is IServiceCollection
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
- #### Liyanjie.MongoDB.Driver.Extensions
    MongoDB Driver的一些扩展
  - MongoDBDateTimeOffsetSerializer
    ```csharp
    BsonSerializer.RegisterSerializer(new MongoDBDateTimeOffsetSerializer());
    ```
  - Extension Methods
    ```csharp
    IMongoQueryable<TSource> IfWhere<TSource>(this IMongoQueryable<TSource> source, Func<bool> ifPredicate, Expression<Func<TSource, bool>> wherePredicate);
    ```
- #### Liyanjie.PhoneNumber.Cn
    手机号码归属地信息
  - PNCnHelper
    ```csharp
    bool TryFind(string phoneNumber, out PhoneNumber number);
    ```
- #### Liyanjie.Pinyin
    汉语拼音
  - PinyinHelper
    ```csharp
    string[] GetPinyins(string input);
    string GetPinyinInitials(string input)
    ```
- #### Liyanjie.Pinyin.Jieba
    汉语拼音
  - PinyinJiebaHelper
    ```csharp
    string[] GetPinyins(string input);
    ```
- #### Liyanjie.Protobuf.Surrogates
- #### Liyanjie.QQWry
  - QQWryHelper
    ```csharp
    (string? Area, string? ISP) SearchIP(string ip);
    ```
- #### Liyanjie.TypeBuilder
    动态构建Type类型
  - TypeFactory
    ```csharp
    Type CreateType(IDictionary<string, Type> properties);  //使用 属性 字典创建类型
    object CreateObject(IDictionary<string, object> values);  //使用 值 字典创建对象
    ```
- #### Liyanjie.Utilities
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
  - Extension Methods
    ```csharp
    //so many…
    ```
- #### Liyanjie.Utilities.Cn
    中国(中文)常用的一些帮助类及扩展方法
  - ChineseADHelper
    ```csharp
    bool TryGetChildren(string code, out Dictionary<string, string> children);
    string[] Display(string code);
    ```
  - ChineseCharHelper
    ```csharp
    bool TryGetChineseCharInfo(char chineseChar, out (string Unicode, int StrokeCount, string[] Pinyins) info);
    ```
  - IPHelper
    ```csharp
    (string Area, string ISP) SearchIP(string ip);
    ```
  - PhoneNumberHelper
    ```csharp
    bool TryFindPhoneNumber(string phoneNumber, out PhoneNumber number);
    ```
  - PinyinHelper
    ```csharp
    string[] GetPinyin(this IEnumerable<string> chineseWords);  //获取中文拼音
    string[] GetChineseWordPinyin(this string chineseWord);  //尝试获取中文词语的拼音
    string[] GetChineseCharPinyins(this char chineseChar);  //尝试获取中文字符的拼音
    ```
  - Extension Methods
    ```csharp
    string ToCn(this number number, OutputType outputType);  //将数字转换为中文
    string ChangeToZhHans(this string zhHantInput);  //中文繁体转简体
    string ChangeToZhHant(this string zhHansInput);  //中文简体转繁体
    bool IsIdNumber_Cn(this string input);  //是否为身份证号码
    bool IsPhoneNumber_Cn(this string input);  //是否为手机号码
    string HideIdNumber_Cn(this string origin, char @char = '*');  //隐藏身份证号码
    string HidePhoneNumber_Cn(this string origin, char @char = '*');  //隐藏手机号码
    ```
- #### Liyanjie.ValueObjects
    常用复合类型
  - Address
  - Consignee
  - Contact
  - Delivery
  - Geolocation
  - Identity
  - Job
  - Licence
  - Name
  - Operator
  - School
  - Score
  - Status
- #### Liyanjie.ValueObjects.Protobuf