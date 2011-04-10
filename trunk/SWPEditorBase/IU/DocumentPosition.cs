/**********************************************
This Program was written by Dario Romero A.
It is licensed under the Apache License v.2.0.
***********************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace SWPEditor.IU
{
    public struct DocumentPosition
    {
        public int Page { get; private set; }
        public int Line { get; private set; }
        public int Character { get; private set; }
        public DocumentPosition(int page, int line, int character):this()
        {
            Page=page;Line=line;Character=character;
        }
    }
}
