// 0x20	space
// 0x3d	=
// 0x78	x
// 0xd	CR


JP :main_init
//JP :count_init

// Test counting from 0 to 99

count_init:
	LD H, 0x0
	LD C, 0x64
	LD L, 0XD
count_loop:
	LD A, H
	CALL :print_number_2digit
	LD A, 0x20
	OUT 0, A
	OUT 0, L
	
	INC H
	
	// if(H == 100) count_init else count_loop
	LD B, H
	SUB B, C
	JP Z, :count_init
	JP :count_loop

	
	
	

	
// Test multiplication

main_init:
	LD H, 0x0
	LD L, 0x0

main_loop:
	LD A, H
	LD B, L
	CALL :multiply
	LD C, A

	// print first number
	LD A, H
	CALL :print_number_2digit

	// print 'x'	
	LD A, 0x78
	OUT 0, A

	// print second number
	LD A, L
	CALL :print_number_2digit

	// print '='	
	LD A, 0x3d
	OUT 0, A

	// print result
	LD A, C
	CALL :print_number_2digit

	// next line	
	LD A, 0xd
	OUT 0, A
	
	INC L
	// if(L == 11) next_h
	LD A, L
	LD C, 0xb
	SUB A, C
	JP Z, :next_h
	JP :main_loop

next_h:
	LD L, 0x0
	INC H
	// if(H == 11) main_end
	LD A, H
	LD C, 0xb
	SUB A, C
	JP Z, :main_end
	JP :main_loop
	
main_end:
	JP :main_end

	
	
#include	C:\Users\albs_\Source\Repos\jaca\2nd gen - 8 bit cpu\ASM source files\Inc_Multiply.asm
#include	C:\Users\albs_\Source\Repos\jaca\2nd gen - 8 bit cpu\ASM source files\Inc_PrintNumber.asm
