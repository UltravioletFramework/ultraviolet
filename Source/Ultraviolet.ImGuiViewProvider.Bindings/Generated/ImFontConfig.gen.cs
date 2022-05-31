using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Ultraviolet.ImGuiViewProvider.Bindings
{
    public unsafe partial struct ImFontConfig
    {
        public void* FontData;
        public int FontDataSize;
        public byte FontDataOwnedByAtlas;
        public int FontNo;
        public float SizePixels;
        public int OversampleH;
        public int OversampleV;
        public byte PixelSnapH;
        public Vector2 GlyphExtraSpacing;
        public Vector2 GlyphOffset;
        public ushort* GlyphRanges;
        public float GlyphMinAdvanceX;
        public float GlyphMaxAdvanceX;
        public byte MergeMode;
        public uint RasterizerFlags;
        public float RasterizerMultiply;
        public fixed byte Name[40];
        public ImFont* DstFont;
    }
    public unsafe partial struct ImFontConfigPtr
    {
        public ImFontConfig* NativePtr { get; }
        public ImFontConfigPtr(ImFontConfig* nativePtr) => NativePtr = nativePtr;
        public ImFontConfigPtr(IntPtr nativePtr) => NativePtr = (ImFontConfig*)nativePtr;
        public static implicit operator ImFontConfigPtr(ImFontConfig* nativePtr) => new ImFontConfigPtr(nativePtr);
        public static implicit operator ImFontConfig* (ImFontConfigPtr wrappedPtr) => wrappedPtr.NativePtr;
        public static implicit operator ImFontConfigPtr(IntPtr nativePtr) => new ImFontConfigPtr(nativePtr);
        public IntPtr FontData { get => (IntPtr)NativePtr->FontData; set => NativePtr->FontData = (void*)value; }
        public ref int FontDataSize => ref Unsafe.AsRef<int>(&NativePtr->FontDataSize);
        public ref Bool8 FontDataOwnedByAtlas => ref Unsafe.AsRef<Bool8>(&NativePtr->FontDataOwnedByAtlas);
        public ref int FontNo => ref Unsafe.AsRef<int>(&NativePtr->FontNo);
        public ref float SizePixels => ref Unsafe.AsRef<float>(&NativePtr->SizePixels);
        public ref int OversampleH => ref Unsafe.AsRef<int>(&NativePtr->OversampleH);
        public ref int OversampleV => ref Unsafe.AsRef<int>(&NativePtr->OversampleV);
        public ref Bool8 PixelSnapH => ref Unsafe.AsRef<Bool8>(&NativePtr->PixelSnapH);
        public ref Vector2 GlyphExtraSpacing => ref Unsafe.AsRef<Vector2>(&NativePtr->GlyphExtraSpacing);
        public ref Vector2 GlyphOffset => ref Unsafe.AsRef<Vector2>(&NativePtr->GlyphOffset);
        public IntPtr GlyphRanges { get => (IntPtr)NativePtr->GlyphRanges; set => NativePtr->GlyphRanges = (ushort*)value; }
        public ref float GlyphMinAdvanceX => ref Unsafe.AsRef<float>(&NativePtr->GlyphMinAdvanceX);
        public ref float GlyphMaxAdvanceX => ref Unsafe.AsRef<float>(&NativePtr->GlyphMaxAdvanceX);
        public ref Bool8 MergeMode => ref Unsafe.AsRef<Bool8>(&NativePtr->MergeMode);
        public ref uint RasterizerFlags => ref Unsafe.AsRef<uint>(&NativePtr->RasterizerFlags);
        public ref float RasterizerMultiply => ref Unsafe.AsRef<float>(&NativePtr->RasterizerMultiply);
        public RangeAccessor<byte> Name => new RangeAccessor<byte>(NativePtr->Name, 40);
        public ImFontPtr DstFont => new ImFontPtr(NativePtr->DstFont);
    }
}
