// Test the #org directive

	LD A, 7
	CALL :print_number_1digit
	jp 0



#org	0x00a

// print out number in A to LCD display
// (0 <= A <=9)
print_number_1digit:
	LD F, 0x30		// ASCII for '0'
	ADD A, F
	OUT 0, A
	RET
