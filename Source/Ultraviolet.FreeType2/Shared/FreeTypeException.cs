using System;
using Ultraviolet.FreeType2.Native;

namespace Ultraviolet.FreeType2
{
    /// <summary>
    /// Represents an exception thrown as a result of a FreeType2 API error.
    /// </summary>
    [Serializable]
    public sealed class FreeTypeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FreeTypeException"/> class.
        /// </summary>
        public FreeTypeException(FT_Error err)
            : base(GetErrorString(err))
        {

        }

        /// <summary>
        /// Gets the error string for the specified error code.
        /// </summary>
        private static String GetErrorString(FT_Error err)
        {
            switch (err)
            {
                case FT_Error.FT_Err_Ok:
                    return "no error";

                case FT_Error.FT_Err_Cannot_Open_Resource:
                    return "cannot open resource";

                case FT_Error.FT_Err_Unknown_File_Format:
                    return "unknown file format";

                case FT_Error.FT_Err_Invalid_File_Format:
                    return "broken file";

                case FT_Error.FT_Err_Invalid_Version:
                    return "invalid FreeType version";

                case FT_Error.FT_Err_Lower_Module_Version:
                    return "module version is too low";

                case FT_Error.FT_Err_Invalid_Argument:
                    return "invalid argument";

                case FT_Error.FT_Err_Unimplemented_Feature:
                    return "unimplemented feature";

                case FT_Error.FT_Err_Invalid_Table:
                    return "broken table";

                case FT_Error.FT_Err_Invalid_Offset:
                    return "broken offset within table";

                case FT_Error.FT_Err_Array_Too_Large:
                    return "array allocation size too large";

                case FT_Error.FT_Err_Missing_Module:
                    return "missing module";

                case FT_Error.FT_Err_Missing_Property:
                    return "missing property";

                case FT_Error.FT_Err_Invalid_Glyph_Index:
                    return "invalid glyph index";

                case FT_Error.FT_Err_Invalid_Character_Code:
                    return "invalid character code";

                case FT_Error.FT_Err_Invalid_Glyph_Format:
                    return "invalid glyph format";

                case FT_Error.FT_Err_Cannot_Render_Glyph:
                    return "cannot render this glyph format";

                case FT_Error.FT_Err_Invalid_Outline:
                    return "invalid outline";

                case FT_Error.FT_Err_Invalid_Composite:
                    return "invalid composite glyph";

                case FT_Error.FT_Err_Too_Many_Hints:
                    return "too many hints";

                case FT_Error.FT_Err_Invalid_Pixel_Size:
                    return "invalid pixel size";

                case FT_Error.FT_Err_Invalid_Handle:
                    return "invalid object handle";

                case FT_Error.FT_Err_Invalid_Library_Handle:
                    return "invalid library handle";

                case FT_Error.FT_Err_Invalid_Driver_Handle:
                    return "invalid module handle";

                case FT_Error.FT_Err_Invalid_Face_Handle:
                    return "invalid face handle";

                case FT_Error.FT_Err_Invalid_Size_Handle:
                    return "invalid size handle";

                case FT_Error.FT_Err_Invalid_Slot_Handle:
                    return "invalid glyph slot handle";

                case FT_Error.FT_Err_Invalid_CharMap_Handle:
                    return "invalid charmap handle";

                case FT_Error.FT_Err_Invalid_Cache_Handle:
                    return "invalid cache manager handle";

                case FT_Error.FT_Err_Invalid_Stream_Handle:
                    return "invalid stream handle";

                case FT_Error.FT_Err_Too_Many_Drivers:
                    return "too many modules";

                case FT_Error.FT_Err_Too_Many_Extensions:
                    return "too many extensions";

                case FT_Error.FT_Err_Out_Of_Memory:
                    return "out of memory";

                case FT_Error.FT_Err_Unlisted_Object:
                    return "unlisted object";

                case FT_Error.FT_Err_Cannot_Open_Stream:
                    return "cannot open stream";

                case FT_Error.FT_Err_Invalid_Stream_Seek:
                    return "invalid stream seek";

                case FT_Error.FT_Err_Invalid_Stream_Skip:
                    return "invalid stream skip";

                case FT_Error.FT_Err_Invalid_Stream_Read:
                    return "invalid stream read";

                case FT_Error.FT_Err_Invalid_Stream_Operation:
                    return "invalid stream operation";

                case FT_Error.FT_Err_Invalid_Frame_Operation:
                    return "invalid frame operation";

                case FT_Error.FT_Err_Nested_Frame_Access:
                    return "nested frame access";

                case FT_Error.FT_Err_Invalid_Frame_Read:
                    return "invalid frame read";

                case FT_Error.FT_Err_Raster_Uninitialized:
                    return "raster uninitialized";

                case FT_Error.FT_Err_Raster_Corrupted:
                    return "raster corrupted";

                case FT_Error.FT_Err_Raster_Overflow:
                    return "raster overflow";

                case FT_Error.FT_Err_Raster_Negative_Height:
                    return "negative height while rastering";

                case FT_Error.FT_Err_Too_Many_Caches:
                    return "too many registered caches";

                case FT_Error.FT_Err_Invalid_Opcode:
                    return "invalid opcode";

                case FT_Error.FT_Err_Too_Few_Arguments:
                    return "too few arguments";

                case FT_Error.FT_Err_Stack_Overflow:
                    return "stack overflow";

                case FT_Error.FT_Err_Code_Overflow:
                    return "code overflow";

                case FT_Error.FT_Err_Bad_Argument:
                    return "bad argument";

                case FT_Error.FT_Err_Divide_By_Zero:
                    return "division by zero";

                case FT_Error.FT_Err_Invalid_Reference:
                    return "invalid reference";

                case FT_Error.FT_Err_Debug_OpCode:
                    return "found debug opcode";

                case FT_Error.FT_Err_ENDF_In_Exec_Stream:
                    return "found ENDF opcode in execution stream";

                case FT_Error.FT_Err_Nested_DEFS:
                    return "nested DEFS";

                case FT_Error.FT_Err_Invalid_CodeRange:
                    return "invalid code range";

                case FT_Error.FT_Err_Execution_Too_Long:
                    return "execution context too long";

                case FT_Error.FT_Err_Too_Many_Function_Defs:
                    return "too many function definitions";

                case FT_Error.FT_Err_Too_Many_Instruction_Defs:
                    return "too many instruction definitions";

                case FT_Error.FT_Err_Table_Missing:
                    return "SFNT font table missing";

                case FT_Error.FT_Err_Horiz_Header_Missing:
                    return "horizontal header (hhea) table missing";

                case FT_Error.FT_Err_Locations_Missing:
                    return "locations (loca) table missing";

                case FT_Error.FT_Err_Name_Table_Missing:
                    return "name table missing";

                case FT_Error.FT_Err_CMap_Table_Missing:
                    return "character map (cmap) table missing";

                case FT_Error.FT_Err_Hmtx_Table_Missing:
                    return "horizontal metrics (hmtx) table missing";

                case FT_Error.FT_Err_Post_Table_Missing:
                    return "PostScript (post) table missing";

                case FT_Error.FT_Err_Invalid_Horiz_Metrics:
                    return "invalid horizontal metrics";

                case FT_Error.FT_Err_Invalid_CharMap_Format:
                    return "invalid character map (cmap) format";

                case FT_Error.FT_Err_Invalid_PPem:
                    return "invalid ppem value";

                case FT_Error.FT_Err_Invalid_Vert_Metrics:
                    return "invalid vertical metrics";

                case FT_Error.FT_Err_Could_Not_Find_Context:
                    return "could not find context";

                case FT_Error.FT_Err_Invalid_Post_Table_Format:
                    return "invalid PostScript (post) table format";

                case FT_Error.FT_Err_Invalid_Post_Table:
                    return "invalid PostScript (post) table";

                case FT_Error.FT_Err_DEF_In_Glyf_Bytecode:
                    return "found FDEF or IDEF opcode in glyf bytecode";

                case FT_Error.FT_Err_Missing_Bitmap:
                    return "missing bitmap in strike";

                case FT_Error.FT_Err_Syntax_Error:
                    return "opcode syntax error";

                case FT_Error.FT_Err_Stack_Underflow:
                    return "argument stack underflow";

                case FT_Error.FT_Err_Ignore:
                    return "ignore";

                case FT_Error.FT_Err_No_Unicode_Glyph_Name:
                    return "no Unicode glyph name found";

                case FT_Error.FT_Err_Glyph_Too_Big:
                    return "glyph too big for hinting";

                case FT_Error.FT_Err_Missing_Startfont_Field:
                    return "`STARTFONT' field missing";

                case FT_Error.FT_Err_Missing_Font_Field:
                    return "`FONT' field missing";

                case FT_Error.FT_Err_Missing_Size_Field:
                    return "`SIZE' field missing";

                case FT_Error.FT_Err_Missing_Fontboundingbox_Field:
                    return "`FONTBOUNDINGBOX' field missing";

                case FT_Error.FT_Err_Missing_Chars_Field:
                    return "`CHARS' field missing";

                case FT_Error.FT_Err_Missing_Startchar_Field:
                    return "`STARTCHAR' field missing";

                case FT_Error.FT_Err_Missing_Encoding_Field:
                    return "`ENCODING' field missing";

                case FT_Error.FT_Err_Missing_Bbx_Field:
                    return "`BBX' field missing";

                case FT_Error.FT_Err_Bbx_Too_Big:
                    return "`BBX' too big";

                case FT_Error.FT_Err_Corrupted_Font_Header:
                    return "Font header corrupted or missing fields";

                case FT_Error.FT_Err_Corrupted_Font_Glyphs:
                    return "Font glyphs corrupted or missing fields";

                default:
                    return "unknown error";
            }
        }
    }
}