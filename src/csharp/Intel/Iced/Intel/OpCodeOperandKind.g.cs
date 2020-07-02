/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

// ⚠️This file was generated by GENERATOR!🦹‍♂️

#nullable enable

#if ENCODER && OPCODE_INFO
namespace Iced.Intel {
	/// <summary>Operand kind</summary>
	public enum OpCodeOperandKind {
		/// <summary>No operand</summary>
		None = 0,
		/// <summary>Far branch 16-bit offset, 16-bit segment/selector</summary>
		farbr2_2 = 1,
		/// <summary>Far branch 32-bit offset, 16-bit segment/selector</summary>
		farbr4_2 = 2,
		/// <summary>Memory offset without a modrm byte (eg. <c>MOV AL,[offset]</c>)</summary>
		mem_offs = 3,
		/// <summary>Memory (modrm)</summary>
		mem = 4,
		/// <summary>Memory (modrm), MPX:<br/>
		/// <br/>
		/// 16/32-bit mode: must be 32-bit addressing<br/>
		/// <br/>
		/// 64-bit mode: 64-bit addressing is forced and must not be RIP relative</summary>
		mem_mpx = 5,
		/// <summary>Memory (modrm), MPX:<br/>
		/// <br/>
		/// 16/32-bit mode: must be 32-bit addressing<br/>
		/// <br/>
		/// 64-bit mode: 64-bit addressing is forced and must not be RIP relative</summary>
		mem_mib = 6,
		/// <summary>Memory (modrm), vsib32, xmm registers</summary>
		mem_vsib32x = 7,
		/// <summary>Memory (modrm), vsib64, xmm registers</summary>
		mem_vsib64x = 8,
		/// <summary>Memory (modrm), vsib32, ymm registers</summary>
		mem_vsib32y = 9,
		/// <summary>Memory (modrm), vsib64, ymm registers</summary>
		mem_vsib64y = 10,
		/// <summary>Memory (modrm), vsib32, zmm registers</summary>
		mem_vsib32z = 11,
		/// <summary>Memory (modrm), vsib64, zmm registers</summary>
		mem_vsib64z = 12,
		/// <summary>8-bit GPR or memory</summary>
		r8_or_mem = 13,
		/// <summary>16-bit GPR or memory</summary>
		r16_or_mem = 14,
		/// <summary>32-bit GPR or memory</summary>
		r32_or_mem = 15,
		/// <summary>32-bit GPR or memory, MPX: 16/32-bit mode: must be 32-bit addressing, 64-bit mode: 64-bit addressing is forced</summary>
		r32_or_mem_mpx = 16,
		/// <summary>64-bit GPR or memory</summary>
		r64_or_mem = 17,
		/// <summary>64-bit GPR or memory, MPX: 16/32-bit mode: must be 32-bit addressing, 64-bit mode: 64-bit addressing is forced</summary>
		r64_or_mem_mpx = 18,
		/// <summary>MM register or memory</summary>
		mm_or_mem = 19,
		/// <summary>XMM register or memory</summary>
		xmm_or_mem = 20,
		/// <summary>YMM register or memory</summary>
		ymm_or_mem = 21,
		/// <summary>ZMM register or memory</summary>
		zmm_or_mem = 22,
		/// <summary>BND register or memory, MPX: 16/32-bit mode: must be 32-bit addressing, 64-bit mode: 64-bit addressing is forced</summary>
		bnd_or_mem_mpx = 23,
		/// <summary>K register or memory</summary>
		k_or_mem = 24,
		/// <summary>8-bit GPR encoded in the <c>reg</c> field of the modrm byte</summary>
		r8_reg = 25,
		/// <summary>8-bit GPR encoded in the low 3 bits of the opcode</summary>
		r8_opcode = 26,
		/// <summary>16-bit GPR encoded in the <c>reg</c> field of the modrm byte</summary>
		r16_reg = 27,
		/// <summary>16-bit GPR encoded in the <c>reg</c> field of the modrm byte. This is a memory operand and it uses the address size prefix (<c>67h</c>) not the operand size prefix (<c>66h</c>).</summary>
		r16_reg_mem = 28,
		/// <summary>16-bit GPR encoded in the <c>mod + r/m</c> fields of the modrm byte</summary>
		r16_rm = 29,
		/// <summary>16-bit GPR encoded in the low 3 bits of the opcode</summary>
		r16_opcode = 30,
		/// <summary>32-bit GPR encoded in the <c>reg</c> field of the modrm byte</summary>
		r32_reg = 31,
		/// <summary>32-bit GPR encoded in the <c>reg</c> field of the modrm byte. This is a memory operand and it uses the address size prefix (<c>67h</c>) not the operand size prefix (<c>66h</c>).</summary>
		r32_reg_mem = 32,
		/// <summary>32-bit GPR encoded in the <c>mod + r/m</c> fields of the modrm byte</summary>
		r32_rm = 33,
		/// <summary>32-bit GPR encoded in the low 3 bits of the opcode</summary>
		r32_opcode = 34,
		/// <summary>32-bit GPR encoded in the the <c>V&apos;vvvv</c> field (VEX/EVEX/XOP)</summary>
		r32_vvvv = 35,
		/// <summary>64-bit GPR encoded in the <c>reg</c> field of the modrm byte</summary>
		r64_reg = 36,
		/// <summary>64-bit GPR encoded in the <c>reg</c> field of the modrm byte. This is a memory operand and it uses the address size prefix (<c>67h</c>) not the operand size prefix (<c>66h</c>).</summary>
		r64_reg_mem = 37,
		/// <summary>64-bit GPR encoded in the <c>mod + r/m</c> fields of the modrm byte</summary>
		r64_rm = 38,
		/// <summary>64-bit GPR encoded in the low 3 bits of the opcode</summary>
		r64_opcode = 39,
		/// <summary>64-bit GPR encoded in the the <c>V&apos;vvvv</c> field (VEX/EVEX/XOP)</summary>
		r64_vvvv = 40,
		/// <summary>Segment register encoded in the <c>reg</c> field of the modrm byte</summary>
		seg_reg = 41,
		/// <summary>K register encoded in the <c>reg</c> field of the modrm byte</summary>
		k_reg = 42,
		/// <summary>K register (+1) encoded in the <c>reg</c> field of the modrm byte</summary>
		kp1_reg = 43,
		/// <summary>K register encoded in the <c>mod + r/m</c> fields of the modrm byte</summary>
		k_rm = 44,
		/// <summary>K register encoded in the the <c>V&apos;vvvv</c> field (VEX/EVEX/XOP)</summary>
		k_vvvv = 45,
		/// <summary>MM register encoded in the <c>reg</c> field of the modrm byte</summary>
		mm_reg = 46,
		/// <summary>MM register encoded in the <c>mod + r/m</c> fields of the modrm byte</summary>
		mm_rm = 47,
		/// <summary>XMM register encoded in the <c>reg</c> field of the modrm byte</summary>
		xmm_reg = 48,
		/// <summary>XMM register encoded in the <c>mod + r/m</c> fields of the modrm byte</summary>
		xmm_rm = 49,
		/// <summary>XMM register encoded in the the <c>V&apos;vvvv</c> field (VEX/EVEX/XOP)</summary>
		xmm_vvvv = 50,
		/// <summary>XMM register (+3) encoded in the the <c>V&apos;vvvv</c> field (VEX/EVEX/XOP)</summary>
		xmmp3_vvvv = 51,
		/// <summary>XMM register encoded in the the high 4 bits of the last 8-bit immediate (VEX/XOP only so only XMM0-XMM15)</summary>
		xmm_is4 = 52,
		/// <summary>XMM register encoded in the the high 4 bits of the last 8-bit immediate (VEX/XOP only so only XMM0-XMM15)</summary>
		xmm_is5 = 53,
		/// <summary>YMM register encoded in the <c>reg</c> field of the modrm byte</summary>
		ymm_reg = 54,
		/// <summary>YMM register encoded in the <c>mod + r/m</c> fields of the modrm byte</summary>
		ymm_rm = 55,
		/// <summary>YMM register encoded in the the <c>V&apos;vvvv</c> field (VEX/EVEX/XOP)</summary>
		ymm_vvvv = 56,
		/// <summary>YMM register encoded in the the high 4 bits of the last 8-bit immediate (VEX/XOP only so only YMM0-YMM15)</summary>
		ymm_is4 = 57,
		/// <summary>YMM register encoded in the the high 4 bits of the last 8-bit immediate (VEX/XOP only so only YMM0-YMM15)</summary>
		ymm_is5 = 58,
		/// <summary>ZMM register encoded in the <c>reg</c> field of the modrm byte</summary>
		zmm_reg = 59,
		/// <summary>ZMM register encoded in the <c>mod + r/m</c> fields of the modrm byte</summary>
		zmm_rm = 60,
		/// <summary>ZMM register encoded in the the <c>V&apos;vvvv</c> field (VEX/EVEX/XOP)</summary>
		zmm_vvvv = 61,
		/// <summary>ZMM register (+3) encoded in the the <c>V&apos;vvvv</c> field (VEX/EVEX/XOP)</summary>
		zmmp3_vvvv = 62,
		/// <summary>CR register encoded in the <c>reg</c> field of the modrm byte</summary>
		cr_reg = 63,
		/// <summary>DR register encoded in the <c>reg</c> field of the modrm byte</summary>
		dr_reg = 64,
		/// <summary>TR register encoded in the <c>reg</c> field of the modrm byte</summary>
		tr_reg = 65,
		/// <summary>BND register encoded in the <c>reg</c> field of the modrm byte</summary>
		bnd_reg = 66,
		/// <summary>ES register</summary>
		es = 67,
		/// <summary>CS register</summary>
		cs = 68,
		/// <summary>SS register</summary>
		ss = 69,
		/// <summary>DS register</summary>
		ds = 70,
		/// <summary>FS register</summary>
		fs = 71,
		/// <summary>GS register</summary>
		gs = 72,
		/// <summary>AL register</summary>
		al = 73,
		/// <summary>CL register</summary>
		cl = 74,
		/// <summary>AX register</summary>
		ax = 75,
		/// <summary>DX register</summary>
		dx = 76,
		/// <summary>EAX register</summary>
		eax = 77,
		/// <summary>RAX register</summary>
		rax = 78,
		/// <summary>ST0 register</summary>
		st0 = 79,
		/// <summary>ST(i) register encoded in the low 3 bits of the opcode</summary>
		sti_opcode = 80,
		/// <summary>2-bit immediate (m2z field, low 2 bits of the /is5 immediate, eg. <c>VPERMIL2PS</c>)</summary>
		imm2_m2z = 81,
		/// <summary>8-bit immediate</summary>
		imm8 = 82,
		/// <summary>Constant 1 (8-bit immediate)</summary>
		imm8_const_1 = 83,
		/// <summary>8-bit immediate sign extended to 16 bits</summary>
		imm8sex16 = 84,
		/// <summary>8-bit immediate sign extended to 32 bits</summary>
		imm8sex32 = 85,
		/// <summary>8-bit immediate sign extended to 64 bits</summary>
		imm8sex64 = 86,
		/// <summary>16-bit immediate</summary>
		imm16 = 87,
		/// <summary>32-bit immediate</summary>
		imm32 = 88,
		/// <summary>32-bit immediate sign extended to 64 bits</summary>
		imm32sex64 = 89,
		/// <summary>64-bit immediate</summary>
		imm64 = 90,
		/// <summary><c>seg:[rSI]</c> memory operand (string instructions)</summary>
		seg_rSI = 91,
		/// <summary><c>es:[rDI]</c> memory operand (string instructions)</summary>
		es_rDI = 92,
		/// <summary><c>seg:[rDI]</c> memory operand (<c>(V)MASKMOVQ</c> instructions)</summary>
		seg_rDI = 93,
		/// <summary><c>seg:[rBX+al]</c> memory operand (<c>XLATB</c> instruction)</summary>
		seg_rBX_al = 94,
		/// <summary>16-bit branch, 1-byte signed relative offset</summary>
		br16_1 = 95,
		/// <summary>32-bit branch, 1-byte signed relative offset</summary>
		br32_1 = 96,
		/// <summary>64-bit branch, 1-byte signed relative offset</summary>
		br64_1 = 97,
		/// <summary>16-bit branch, 2-byte signed relative offset</summary>
		br16_2 = 98,
		/// <summary>32-bit branch, 4-byte signed relative offset</summary>
		br32_4 = 99,
		/// <summary>64-bit branch, 4-byte signed relative offset</summary>
		br64_4 = 100,
		/// <summary><c>XBEGIN</c>, 2-byte signed relative offset</summary>
		xbegin_2 = 101,
		/// <summary><c>XBEGIN</c>, 4-byte signed relative offset</summary>
		xbegin_4 = 102,
		/// <summary>2-byte branch offset (<c>JMPE</c> instruction)</summary>
		brdisp_2 = 103,
		/// <summary>4-byte branch offset (<c>JMPE</c> instruction)</summary>
		brdisp_4 = 104,
		/// <summary>Memory (modrm) and the sib byte must be present</summary>
		sibmem = 105,
		/// <summary>TMM register encoded in the <c>reg</c> field of the modrm byte</summary>
		tmm_reg = 106,
		/// <summary>TMM register encoded in the <c>mod + r/m</c> fields of the modrm byte</summary>
		tmm_rm = 107,
		/// <summary>TMM register encoded in the the <c>V&apos;vvvv</c> field (VEX/EVEX/XOP)</summary>
		tmm_vvvv = 108,
	}
}
#endif
