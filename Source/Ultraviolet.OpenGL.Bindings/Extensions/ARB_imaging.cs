using System;
using System.Runtime.InteropServices;
using Ultraviolet.Core;

namespace Ultraviolet.OpenGL.Bindings
{
    public static unsafe partial class GL
    {
        [MonoNativeFunctionWrapper]
        private delegate void glColorSubTableDelegate(uint target, int start, int count, uint format, uint type, IntPtr data);
        [Require(Extension = "GL_ARB_imaging")]
        private static glColorSubTableDelegate glColorSubTable = null;

        public static void ColorSubTable(uint target, int start, int count, uint format, uint type, void* data) { glColorSubTable(target, start, count, format, type, (IntPtr)data); }

        [MonoNativeFunctionWrapper]
        private delegate void glColorTableDelegate(uint target, uint internalformat, int width, uint format, uint type, IntPtr table);
        [Require(Extension = "GL_ARB_imaging")]
        private static glColorTableDelegate glColorTable = null;

        public static void ColorTable(uint target, uint internalformat, int width, uint format, uint type, void* table) { glColorTable(target, internalformat, width, format, type, (IntPtr)table); }

        [MonoNativeFunctionWrapper]
        private delegate void glColorTableParameterfvDelegate(uint target, uint pname, IntPtr @params);
        [Require(Extension = "GL_ARB_imaging")]
        private static glColorTableParameterfvDelegate glColorTableParameterfv = null;

        public static void ColorTableParameterfv(uint target, uint pname, float* @params) { glColorTableParameterfv(target, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glColorTableParameterivDelegate(uint target, uint pname, IntPtr @params);
        [Require(Extension = "GL_ARB_imaging")]
        private static glColorTableParameterivDelegate glColorTableParameteriv = null;

        public static void ColorTableParameteriv(uint target, uint pname, int* @params) { glColorTableParameteriv(target, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glConvolutionFilter1DDelegate(uint target, uint internalformat, int width, uint format, uint type, IntPtr image);
        [Require(Extension = "GL_ARB_imaging")]
        private static glConvolutionFilter1DDelegate glConvolutionFilter1D = null;

        public static void ConvolutionFilter1D(uint target, uint internalformat, int width, uint format, uint type, void* image) { glConvolutionFilter1D(target, internalformat, width, format, type, (IntPtr)image); }

        [MonoNativeFunctionWrapper]
        private delegate void glConvolutionFilter2DDelegate(uint target, uint internalformat, int width, int height, uint format, uint type, IntPtr image);
        [Require(Extension = "GL_ARB_imaging")]
        private static glConvolutionFilter2DDelegate glConvolutionFilter2D = null;

        public static void ConvolutionFilter2D(uint target, uint internalformat, int width, int height, uint format, uint type, void* image) { glConvolutionFilter2D(target, internalformat, width, height, format, type, (IntPtr)image); }

        [MonoNativeFunctionWrapper]
        private delegate void glConvolutionParameterfDelegate(uint target, uint pname, float @params);
        [Require(Extension = "GL_ARB_imaging")]
        private static glConvolutionParameterfDelegate glConvolutionParameterf = null;

        public static void ConvolutionParameterf(uint target, uint pname, float @params) { glConvolutionParameterf(target, pname, @params); }

        [MonoNativeFunctionWrapper]
        private delegate void glConvolutionParameterfvDelegate(uint target, uint pname, IntPtr @params);
        [Require(Extension = "GL_ARB_imaging")]
        private static glConvolutionParameterfvDelegate glConvolutionParameterfv = null;

        public static void ConvolutionParameterfv(uint target, uint pname, float* @params) { glConvolutionParameterfv(target, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glConvolutionParameteriDelegate(uint target, uint pname, int @params);
        [Require(Extension = "GL_ARB_imaging")]
        private static glConvolutionParameteriDelegate glConvolutionParameteri = null;

        public static void ConvolutionParameteri(uint target, uint pname, int @params) { glConvolutionParameteri(target, pname, @params); }

        [MonoNativeFunctionWrapper]
        private delegate void glConvolutionParameterivDelegate(uint target, uint pname, IntPtr @params);
        [Require(Extension = "GL_ARB_imaging")]
        private static glConvolutionParameterivDelegate glConvolutionParameteriv = null;

        public static void ConvolutionParameteriv(uint target, uint pname, int* @params) { glConvolutionParameteriv(target, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glCopyColorSubTableDelegate(uint target, int start, int x, int y, int width);
        [Require(Extension = "GL_ARB_imaging")]
        private static glCopyColorSubTableDelegate glCopyColorSubTable = null;

        public static void CopyColorSubTable(uint target, int start, int x, int y, int width) { glCopyColorSubTable(target, start, x, y, width); }

        [MonoNativeFunctionWrapper]
        private delegate void glCopyColorTableDelegate(uint target, uint internalformat, int x, int y, int width);
        [Require(Extension = "GL_ARB_imaging")]
        private static glCopyColorTableDelegate glCopyColorTable = null;

        public static void CopyColorTable(uint target, uint internalformat, int x, int y, int width) { glCopyColorTable(target, internalformat, x, y, width); }

        [MonoNativeFunctionWrapper]
        private delegate void glCopyConvolutionFilter1DDelegate(uint target, uint internalformat, int x, int y, int width);
        [Require(Extension = "GL_ARB_imaging")]
        private static glCopyConvolutionFilter1DDelegate glCopyConvolutionFilter1D = null;

        public static void CopyConvolutionFilter1D(uint target, uint internalformat, int x, int y, int width) { glCopyConvolutionFilter1D(target, internalformat, x, y, width); }

        [MonoNativeFunctionWrapper]
        private delegate void glCopyConvolutionFilter2DDelegate(uint target, uint internalformat, int x, int y, int width, int height);
        [Require(Extension = "GL_ARB_imaging")]
        private static glCopyConvolutionFilter2DDelegate glCopyConvolutionFilter2D = null;

        public static void CopyConvolutionFilter2D(uint target, uint internalformat, int x, int y, int width, int height) { glCopyConvolutionFilter2D(target, internalformat, x, y, width, height); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetColorTableDelegate(uint target, uint format, uint type, IntPtr table);
        [Require(Extension = "GL_ARB_imaging")]
        private static glGetColorTableDelegate glGetColorTable = null;

        public static void GetColorTable(uint target, uint format, uint type, void* table) { glGetColorTable(target, format, type, (IntPtr)table); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetColorTableParameterfvDelegate(uint target, uint pname, IntPtr @params);
        [Require(Extension = "GL_ARB_imaging")]
        private static glGetColorTableParameterfvDelegate glGetColorTableParameterfv = null;

        public static void GetColorTableParameterfv(uint target, uint pname, float* @params) { glGetColorTableParameterfv(target, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetColorTableParameterivDelegate(uint target, uint pname, IntPtr @params);
        [Require(Extension = "GL_ARB_imaging")]
        private static glGetColorTableParameterivDelegate glGetColorTableParameteriv = null;

        public static void GetColorTableParameteriv(uint target, uint pname, int* @params) { glGetColorTableParameteriv(target, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetConvolutionFilterDelegate(uint target, uint format, uint type, IntPtr image);
        [Require(Extension = "GL_ARB_imaging")]
        private static glGetConvolutionFilterDelegate glGetConvolutionFilter = null;

        public static void GetConvolutionFilter(uint target, uint format, uint type, void* image) { glGetConvolutionFilter(target, format, type, (IntPtr)image); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetConvolutionParameterfvDelegate(uint target, uint pname, IntPtr @params);
        [Require(Extension = "GL_ARB_imaging")]
        private static glGetConvolutionParameterfvDelegate glGetConvolutionParameterfv = null;

        public static void GetConvolutionParameterfv(uint target, uint pname, float* @params) { glGetConvolutionParameterfv(target, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetConvolutionParameterivDelegate(uint target, uint pname, IntPtr @params);
        [Require(Extension = "GL_ARB_imaging")]
        private static glGetConvolutionParameterivDelegate glGetConvolutionParameteriv = null;

        public static void GetConvolutionParameteriv(uint target, uint pname, int* @params) { glGetConvolutionParameteriv(target, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetHistogramDelegate(uint target, [MarshalAs(UnmanagedType.I1)] bool reset, uint format, uint type, IntPtr values);
        [Require(Extension = "GL_ARB_imaging")]
        private static glGetHistogramDelegate glGetHistogram = null;

        public static void GetHistogram(uint target, bool reset, uint format, uint type, void* values) { glGetHistogram(target, reset, format, type, (IntPtr)values); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetHistogramParameterfvDelegate(uint target, uint pname, IntPtr @params);
        [Require(Extension = "GL_ARB_imaging")]
        private static glGetHistogramParameterfvDelegate glGetHistogramParameterfv = null;

        public static void GetHistogramParameterfv(uint target, uint pname, float* @params) { glGetHistogramParameterfv(target, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetHistogramParameterivDelegate(uint target, uint pname, IntPtr @params);
        [Require(Extension = "GL_ARB_imaging")]
        private static glGetHistogramParameterivDelegate glGetHistogramParameteriv = null;

        public static void GetHistogramParameteriv(uint target, uint pname, int* @params) { glGetHistogramParameteriv(target, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetMinmaxDelegate(uint target, [MarshalAs(UnmanagedType.I1)] bool reset, uint format, uint types, IntPtr values);
        [Require(Extension = "GL_ARB_imaging")]
        private static glGetMinmaxDelegate glGetMinmax = null;

        public static void GetMinmax(uint target, bool reset, uint format, uint types, void* values) { glGetMinmax(target, reset, format, types, (IntPtr)values); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetMinmaxParameterfvDelegate(uint target, uint pname, IntPtr @params);
        [Require(Extension = "GL_ARB_imaging")]
        private static glGetMinmaxParameterfvDelegate glGetMinmaxParameterfv = null;

        public static void GetMinmaxParameterfv(uint target, uint pname, float* @params) { glGetMinmaxParameterfv(target, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetMinmaxParameterivDelegate(uint target, uint pname, IntPtr @params);
        [Require(Extension = "GL_ARB_imaging")]
        private static glGetMinmaxParameterivDelegate glGetMinmaxParameteriv = null;

        public static void GetMinmaxParameteriv(uint target, uint pname, int* @params) { glGetMinmaxParameteriv(target, pname, (IntPtr)@params); }

        [MonoNativeFunctionWrapper]
        private delegate void glGetSeparableFilterDelegate(uint target, uint format, uint type, IntPtr row, IntPtr column, IntPtr span);
        [Require(Extension = "GL_ARB_imaging")]
        private static glGetSeparableFilterDelegate glGetSeparableFilter = null;

        public static void GetSeparableFilter(uint target, uint format, uint type, void* row, void* column, void* span) { glGetSeparableFilter(target, format, type, (IntPtr)row, (IntPtr)column, (IntPtr)span); }

        [MonoNativeFunctionWrapper]
        private delegate void glHistogramDelegate(uint target, int width, uint internalformat, [MarshalAs(UnmanagedType.I1)] bool sink);
        [Require(Extension = "GL_ARB_imaging")]
        private static glHistogramDelegate glHistogram = null;

        public static void Histogram(uint target, int width, uint internalformat, bool sink) { glHistogram(target, width, internalformat, sink); }

        [MonoNativeFunctionWrapper]
        private delegate void glMinmaxDelegate(uint target, uint internalformat, [MarshalAs(UnmanagedType.I1)] bool sink);
        [Require(Extension = "GL_ARB_imaging")]
        private static glMinmaxDelegate glMinmax = null;

        public static void Minmax(uint target, uint internalformat, bool sink) { glMinmax(target, internalformat, sink); }

        [MonoNativeFunctionWrapper]
        private delegate void glResetHistogramDelegate(uint target);
        [Require(Extension = "GL_ARB_imaging")]
        private static glResetHistogramDelegate glResetHistogram = null;

        public static void ResetHistogram(uint target) { glResetHistogram(target); }

        [MonoNativeFunctionWrapper]
        private delegate void glResetMinmaxDelegate(uint target);
        [Require(Extension = "GL_ARB_imaging")]
        private static glResetMinmaxDelegate glResetMinmax = null;

        public static void ResetMinmax(uint target) { glResetMinmax(target); }

        [MonoNativeFunctionWrapper]
        private delegate void glSeparableFilter2DDelegate(uint target, uint internalformat, int width, int height, uint format, uint type, IntPtr row, IntPtr column);
        [Require(Extension = "GL_ARB_imaging")]
        private static glSeparableFilter2DDelegate glSeparableFilter2D = null;

        public static void SeparableFilter2D(uint target, uint internalformat, int width, int height, uint format, uint type, void* row, void* column) { glSeparableFilter2D(target, internalformat, width, height, format, type, (IntPtr)row, (IntPtr)column); }

        public const UInt32 GL_CONSTANT_COLOR = 0x8001;
        public const UInt32 GL_ONE_MINUS_CONSTANT_COLOR = 0x8002;
        public const UInt32 GL_CONSTANT_ALPHA = 0x8003;
        public const UInt32 GL_ONE_MINUS_CONSTANT_ALPHA = 0x8004;
        public const UInt32 GL_BLEND_COLOR = 0x8005;
        public const UInt32 GL_FUNC_ADD = 0x8006;
        public const UInt32 GL_MIN = 0x8007;
        public const UInt32 GL_MAX = 0x8008;
        public const UInt32 GL_BLEND_EQUATION = 0x8009;
        public const UInt32 GL_FUNC_SUBTRACT = 0x800A;
        public const UInt32 GL_FUNC_REVERSE_SUBTRACT = 0x800B;
        public const UInt32 GL_CONVOLUTION_1D = 0x8010;
        public const UInt32 GL_CONVOLUTION_2D = 0x8011;
        public const UInt32 GL_SEPARABLE_2D = 0x8012;
        public const UInt32 GL_CONVOLUTION_BORDER_MODE = 0x8013;
        public const UInt32 GL_CONVOLUTION_FILTER_SCALE = 0x8014;
        public const UInt32 GL_CONVOLUTION_FILTER_BIAS = 0x8015;
        public const UInt32 GL_REDUCE = 0x8016;
        public const UInt32 GL_CONVOLUTION_FORMAT = 0x8017;
        public const UInt32 GL_CONVOLUTION_WIDTH = 0x8018;
        public const UInt32 GL_CONVOLUTION_HEIGHT = 0x8019;
        public const UInt32 GL_MAX_CONVOLUTION_WIDTH = 0x801A;
        public const UInt32 GL_MAX_CONVOLUTION_HEIGHT = 0x801B;
        public const UInt32 GL_POST_CONVOLUTION_RED_SCALE = 0x801C;
        public const UInt32 GL_POST_CONVOLUTION_GREEN_SCALE = 0x801D;
        public const UInt32 GL_POST_CONVOLUTION_BLUE_SCALE = 0x801E;
        public const UInt32 GL_POST_CONVOLUTION_ALPHA_SCALE = 0x801F;
        public const UInt32 GL_POST_CONVOLUTION_RED_BIAS = 0x8020;
        public const UInt32 GL_POST_CONVOLUTION_GREEN_BIAS = 0x8021;
        public const UInt32 GL_POST_CONVOLUTION_BLUE_BIAS = 0x8022;
        public const UInt32 GL_POST_CONVOLUTION_ALPHA_BIAS = 0x8023;
        public const UInt32 GL_HISTOGRAM = 0x8024;
        public const UInt32 GL_PROXY_HISTOGRAM = 0x8025;
        public const UInt32 GL_HISTOGRAM_WIDTH = 0x8026;
        public const UInt32 GL_HISTOGRAM_FORMAT = 0x8027;
        public const UInt32 GL_HISTOGRAM_RED_SIZE = 0x8028;
        public const UInt32 GL_HISTOGRAM_GREEN_SIZE = 0x8029;
        public const UInt32 GL_HISTOGRAM_BLUE_SIZE = 0x802A;
        public const UInt32 GL_HISTOGRAM_ALPHA_SIZE = 0x802B;
        public const UInt32 GL_HISTOGRAM_LUMINANCE_SIZE = 0x802C;
        public const UInt32 GL_HISTOGRAM_SINK = 0x802D;
        public const UInt32 GL_MINMAX = 0x802E;
        public const UInt32 GL_MINMAX_FORMAT = 0x802F;
        public const UInt32 GL_MINMAX_SINK = 0x8030;
        public const UInt32 GL_TABLE_TOO_LARGE = 0x8031;
        public const UInt32 GL_COLOR_MATRIX = 0x80B1;
        public const UInt32 GL_COLOR_MATRIX_STACK_DEPTH = 0x80B2;
        public const UInt32 GL_MAX_COLOR_MATRIX_STACK_DEPTH = 0x80B3;
        public const UInt32 GL_POST_COLOR_MATRIX_RED_SCALE = 0x80B4;
        public const UInt32 GL_POST_COLOR_MATRIX_GREEN_SCALE = 0x80B5;
        public const UInt32 GL_POST_COLOR_MATRIX_BLUE_SCALE = 0x80B6;
        public const UInt32 GL_POST_COLOR_MATRIX_ALPHA_SCALE = 0x80B7;
        public const UInt32 GL_POST_COLOR_MATRIX_RED_BIAS = 0x80B8;
        public const UInt32 GL_POST_COLOR_MATRIX_GREEN_BIAS = 0x80B9;
        public const UInt32 GL_POST_COLOR_MATRIX_BLUE_BIAS = 0x80BA;
        public const UInt32 GL_POST_COLOR_MATRIX_ALPHA_BIAS = 0x80BB;
        public const UInt32 GL_COLOR_TABLE = 0x80D0;
        public const UInt32 GL_POST_CONVOLUTION_COLOR_TABLE = 0x80D1;
        public const UInt32 GL_POST_COLOR_MATRIX_COLOR_TABLE = 0x80D2;
        public const UInt32 GL_PROXY_COLOR_TABLE = 0x80D3;
        public const UInt32 GL_PROXY_POST_CONVOLUTION_COLOR_TABLE = 0x80D4;
        public const UInt32 GL_PROXY_POST_COLOR_MATRIX_COLOR_TABLE = 0x80D5;
        public const UInt32 GL_COLOR_TABLE_SCALE = 0x80D6;
        public const UInt32 GL_COLOR_TABLE_BIAS = 0x80D7;
        public const UInt32 GL_COLOR_TABLE_FORMAT = 0x80D8;
        public const UInt32 GL_COLOR_TABLE_WIDTH = 0x80D9;
        public const UInt32 GL_COLOR_TABLE_RED_SIZE = 0x80DA;
        public const UInt32 GL_COLOR_TABLE_GREEN_SIZE = 0x80DB;
        public const UInt32 GL_COLOR_TABLE_BLUE_SIZE = 0x80DC;
        public const UInt32 GL_COLOR_TABLE_ALPHA_SIZE = 0x80DD;
        public const UInt32 GL_COLOR_TABLE_LUMINANCE_SIZE = 0x80DE;
        public const UInt32 GL_COLOR_TABLE_INTENSITY_SIZE = 0x80DF;
        public const UInt32 GL_IGNORE_BORDER = 0x8150;
        public const UInt32 GL_CONSTANT_BORDER = 0x8151;
        public const UInt32 GL_WRAP_BORDER = 0x8152;
        public const UInt32 GL_REPLICATE_BORDER = 0x8153;
        public const UInt32 GL_CONVOLUTION_BORDER_COLOR = 0x8154;
    }
}
