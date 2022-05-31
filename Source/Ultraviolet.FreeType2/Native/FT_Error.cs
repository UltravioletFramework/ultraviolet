namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    public enum FT_Error
    {
        /* generic errors */
        FT_Err_Ok                               = 0x00,
        FT_Err_Cannot_Open_Resource             = 0x01,
        FT_Err_Unknown_File_Format              = 0x02,
        FT_Err_Invalid_File_Format              = 0x03,
        FT_Err_Invalid_Version                  = 0x04,
        FT_Err_Lower_Module_Version             = 0x05,
        FT_Err_Invalid_Argument                 = 0x06,
        FT_Err_Unimplemented_Feature            = 0x07,
        FT_Err_Invalid_Table                    = 0x08,
        FT_Err_Invalid_Offset                   = 0x09,
        FT_Err_Array_Too_Large                  = 0x0A,
        FT_Err_Missing_Module                   = 0x0B,
        FT_Err_Missing_Property                 = 0x0C,

        /* glyph/character errors */
        FT_Err_Invalid_Glyph_Index              = 0x10,
        FT_Err_Invalid_Character_Code           = 0x11,
        FT_Err_Invalid_Glyph_Format             = 0x12,
        FT_Err_Cannot_Render_Glyph              = 0x13,
        FT_Err_Invalid_Outline                  = 0x14,
        FT_Err_Invalid_Composite                = 0x15,
        FT_Err_Too_Many_Hints                   = 0x16,
        FT_Err_Invalid_Pixel_Size               = 0x17,

        /* handle errors */
        FT_Err_Invalid_Handle                   = 0x20,
        FT_Err_Invalid_Library_Handle           = 0x21,
        FT_Err_Invalid_Driver_Handle            = 0x22,
        FT_Err_Invalid_Face_Handle              = 0x23,
        FT_Err_Invalid_Size_Handle              = 0x24,
        FT_Err_Invalid_Slot_Handle              = 0x25,
        FT_Err_Invalid_CharMap_Handle           = 0x26,
        FT_Err_Invalid_Cache_Handle             = 0x27,
        FT_Err_Invalid_Stream_Handle            = 0x28,

        /* driver errors */
        FT_Err_Too_Many_Drivers                 = 0x30,
        FT_Err_Too_Many_Extensions              = 0x31,

        /* memory errors */
        FT_Err_Out_Of_Memory                    = 0x40,
        FT_Err_Unlisted_Object                  = 0x41,

        /* stream errors */
        FT_Err_Cannot_Open_Stream               = 0x51,
        FT_Err_Invalid_Stream_Seek              = 0x52,
        FT_Err_Invalid_Stream_Skip              = 0x53,
        FT_Err_Invalid_Stream_Read              = 0x54,
        FT_Err_Invalid_Stream_Operation         = 0x55,
        FT_Err_Invalid_Frame_Operation          = 0x56,
        FT_Err_Nested_Frame_Access              = 0x57,
        FT_Err_Invalid_Frame_Read               = 0x58,

        /* raster errors */
        FT_Err_Raster_Uninitialized             = 0x60,
        FT_Err_Raster_Corrupted                 = 0x61,
        FT_Err_Raster_Overflow                  = 0x62,
        FT_Err_Raster_Negative_Height           = 0x63,

        /* cache errors */
        FT_Err_Too_Many_Caches                  = 0x70,

        /* TrueType and SFNT errors */
        FT_Err_Invalid_Opcode                   = 0x80,
        FT_Err_Too_Few_Arguments                = 0x81,
        FT_Err_Stack_Overflow                   = 0x82,
        FT_Err_Code_Overflow                    = 0x83,
        FT_Err_Bad_Argument                     = 0x84,
        FT_Err_Divide_By_Zero                   = 0x85,
        FT_Err_Invalid_Reference                = 0x86,
        FT_Err_Debug_OpCode                     = 0x87,
        FT_Err_ENDF_In_Exec_Stream              = 0x88,
        FT_Err_Nested_DEFS                      = 0x89,
        FT_Err_Invalid_CodeRange                = 0x8A,
        FT_Err_Execution_Too_Long               = 0x8B,
        FT_Err_Too_Many_Function_Defs           = 0x8C,
        FT_Err_Too_Many_Instruction_Defs        = 0x8D,
        FT_Err_Table_Missing                    = 0x8E,
        FT_Err_Horiz_Header_Missing             = 0x8F,
        FT_Err_Locations_Missing                = 0x90,
        FT_Err_Name_Table_Missing               = 0x91,
        FT_Err_CMap_Table_Missing               = 0x92,
        FT_Err_Hmtx_Table_Missing               = 0x93,
        FT_Err_Post_Table_Missing               = 0x94,
        FT_Err_Invalid_Horiz_Metrics            = 0x95,
        FT_Err_Invalid_CharMap_Format           = 0x96,
        FT_Err_Invalid_PPem                     = 0x97,
        FT_Err_Invalid_Vert_Metrics             = 0x98,
        FT_Err_Could_Not_Find_Context           = 0x99,
        FT_Err_Invalid_Post_Table_Format        = 0x9A,
        FT_Err_Invalid_Post_Table               = 0x9B,
        FT_Err_DEF_In_Glyf_Bytecode             = 0x9C,
        FT_Err_Missing_Bitmap                   = 0x9D,

        /* CFF, CID, and Type 1 errors */
        FT_Err_Syntax_Error                     = 0xA0,
        FT_Err_Stack_Underflow                  = 0xA1,
        FT_Err_Ignore                           = 0xA2,
        FT_Err_No_Unicode_Glyph_Name            = 0xA3,
        FT_Err_Glyph_Too_Big                    = 0xA4,

        /* BDF errors */
        FT_Err_Missing_Startfont_Field          = 0xB0,
        FT_Err_Missing_Font_Field               = 0xB1,
        FT_Err_Missing_Size_Field               = 0xB2,
        FT_Err_Missing_Fontboundingbox_Field    = 0xB3,
        FT_Err_Missing_Chars_Field              = 0xB4,
        FT_Err_Missing_Startchar_Field          = 0xB5,
        FT_Err_Missing_Encoding_Field           = 0xB6,
        FT_Err_Missing_Bbx_Field                = 0xB7,
        FT_Err_Bbx_Too_Big                      = 0xB8,
        FT_Err_Corrupted_Font_Header            = 0xB9,
        FT_Err_Corrupted_Font_Glyphs            = 0xBA,
    }
#pragma warning restore 1591
}
