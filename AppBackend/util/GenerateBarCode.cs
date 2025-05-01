
using ZXing;
using SkiaSharp;
using ZXing.SkiaSharp;
using ZXing.SkiaSharp.Rendering;

public class GenerateBarCode{


public static byte[] GenerateBarcode(string content){
    var writer= new BarcodeWriter
    {
        Format=BarcodeFormat.CODE_128,
        Options=new ZXing.Common.EncodingOptions
        {
            Width=400,
            Height=100,
            Margin=10,
            PureBarcode=false
        },
        Renderer= new SKBitmapRenderer()
    };
    using var bitmap=writer.Write(content);
    using var image= SKImage.FromBitmap(bitmap);
    using var data= image.Encode(SKEncodedImageFormat.Png,100);
    return data.ToArray();
}
}
