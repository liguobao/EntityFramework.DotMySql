﻿#if !(NET45 || NET452 || DNX452)
// Copyright © 2004, 2013, Oracle and/or its affiliates. All rights reserved.
//
// MySQL Connector/NET is licensed under the terms of the GPLv2
// <http://www.gnu.org/licenses/old-licenses/gpl-2.0.html>, like most 
// MySQL Connectors. There are special exceptions to the terms and 
// conditions of the GPLv2 as it is applied to this software, see the 
// FLOSS License Exception
// <http://www.mysql.com/about/legal/licensing/foss-exception.html>.
//
// This program is free software; you can redistribute it and/or modify 
// it under the terms of the GNU General Public License as published 
// by the Free Software Foundation; version 2 of the License.
//
// This program is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License 
// for more details.
//
// You should have received a copy of the GNU General Public License along 
// with this program; if not, write to the Free Software Foundation, Inc., 
// 51 Franklin St, Fifth Floor, Boston, MA 02110-1301  USA

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySql.Data.MySqlClient
{
    /// <summary>
    /// An implementation of BitArray that has the missing CopyTo method in RT.
    /// </summary>
    internal class RtBitArray
    {
        internal RtBitArray(int length)
        {
            buf = new byte[( length + 7 ) >> 3];
            this.length = length;
        }

        internal void CopyTo(byte[] buffer, int targetIdx)
        {
            Array.Copy(buf, 0, buffer, targetIdx, ( length + 7 ) >> 3 );
        }

        internal int Count { get { return length; } }

        private int length;
        private byte[] buf;

        internal bool this[int i]
        {
            get
            {
                return ( buf[i >> 3] & ( byte )(1 << (i & 7)) ) != 0;
            }
            set
            {
                if (value)
                    buf[i >> 3] |= (byte)(1 << (i & 7));
                else
                    buf[i >> 3] &= (byte)~(1 << (i & 7));
            }
        }
    }
}
#endif
