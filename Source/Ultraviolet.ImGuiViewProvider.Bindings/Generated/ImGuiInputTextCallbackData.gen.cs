using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Ultraviolet.ImGuiViewProvider.Bindings
{
    public unsafe partial struct ImGuiInputTextCallbackData
    {
        public ImGuiInputTextFlags EventFlag;
        public ImGuiInputTextFlags Flags;
        public void* UserData;
        public ushort EventChar;
        public ImGuiKey EventKey;
        public byte* Buf;
        public int BufTextLen;
        public int BufSize;
        public byte BufDirty;
        public int CursorPos;
        public int SelectionStart;
        public int SelectionEnd;
    }
    public unsafe partial struct ImGuiInputTextCallbackDataPtr
    {
        public ImGuiInputTextCallbackData* NativePtr { get; }
        public ImGuiInputTextCallbackDataPtr(ImGuiInputTextCallbackData* nativePtr) => NativePtr = nativePtr;
        public ImGuiInputTextCallbackDataPtr(IntPtr nativePtr) => NativePtr = (ImGuiInputTextCallbackData*)nativePtr;
        public static implicit operator ImGuiInputTextCallbackDataPtr(ImGuiInputTextCallbackData* nativePtr) => new ImGuiInputTextCallbackDataPtr(nativePtr);
        public static implicit operator ImGuiInputTextCallbackData* (ImGuiInputTextCallbackDataPtr wrappedPtr) => wrappedPtr.NativePtr;
        public static implicit operator ImGuiInputTextCallbackDataPtr(IntPtr nativePtr) => new ImGuiInputTextCallbackDataPtr(nativePtr);
        public ref ImGuiInputTextFlags EventFlag => ref Unsafe.AsRef<ImGuiInputTextFlags>(&NativePtr->EventFlag);
        public ref ImGuiInputTextFlags Flags => ref Unsafe.AsRef<ImGuiInputTextFlags>(&NativePtr->Flags);
        public IntPtr UserData { get => (IntPtr)NativePtr->UserData; set => NativePtr->UserData = (void*)value; }
        public ref ushort EventChar => ref Unsafe.AsRef<ushort>(&NativePtr->EventChar);
        public ref ImGuiKey EventKey => ref Unsafe.AsRef<ImGuiKey>(&NativePtr->EventKey);
        public IntPtr Buf { get => (IntPtr)NativePtr->Buf; set => NativePtr->Buf = (byte*)value; }
        public ref int BufTextLen => ref Unsafe.AsRef<int>(&NativePtr->BufTextLen);
        public ref int BufSize => ref Unsafe.AsRef<int>(&NativePtr->BufSize);
        public ref Bool8 BufDirty => ref Unsafe.AsRef<Bool8>(&NativePtr->BufDirty);
        public ref int CursorPos => ref Unsafe.AsRef<int>(&NativePtr->CursorPos);
        public ref int SelectionStart => ref Unsafe.AsRef<int>(&NativePtr->SelectionStart);
        public ref int SelectionEnd => ref Unsafe.AsRef<int>(&NativePtr->SelectionEnd);
        public void DeleteChars(int pos, int bytes_count)
        {
            ImGuiNative.ImGuiInputTextCallbackData_DeleteChars(NativePtr, pos, bytes_count);
        }
        public bool HasSelection()
        {
            byte ret = ImGuiNative.ImGuiInputTextCallbackData_HasSelection(NativePtr);
            return ret != 0;
        }
        public void InsertChars(int pos, string text)
        {
            int text_byteCount = Encoding.UTF8.GetByteCount(text);
            byte* native_text = stackalloc byte[text_byteCount + 1];
            fixed (char* text_ptr = text)
            {
                int native_text_offset = Encoding.UTF8.GetBytes(text_ptr, text.Length, native_text, text_byteCount);
                native_text[native_text_offset] = 0;
            }
            byte* native_text_end = null;
            ImGuiNative.ImGuiInputTextCallbackData_InsertChars(NativePtr, pos, native_text, native_text_end);
        }
    }
}
