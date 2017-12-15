// print out number in A to LCD display
// (0 <= A <=9)

#org	0x200

print_number_1digit:
	LD F, 0x30
	ADD A, F
	OUT 0, A
	RET



	
	
// print out number in A register to LCD display
// valid range: 0-99

#org	0x300

print_number_2digit:
	LD E, H
	LD D, 0xa
	// H: Number of tens
	LD H, 0x0

pn_loop:
	//if(A < 10) pn_end else { A -= 10; H++; }
	LD B, A
	SUB A, D
	JP C, :pn_end

	//A -= 10
	SUB B, D
	LD A, B
	INC H
	JP :pn_loop
	
pn_end:
	LD F, 0x30

	DNW H
	JP Z, :pn_last_dig

	ADD H, F
	LD A, H
	OUT 0, A

pn_last_dig:
	ADD B, F
	LD A, B
	OUT 0, A
	
	LD H, E

	RET
