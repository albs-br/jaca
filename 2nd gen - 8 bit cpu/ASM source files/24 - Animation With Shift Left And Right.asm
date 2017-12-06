main_init:

	LD B, 0xff
	LD D, 0x8
	LD L, 0x0

left_start:

	OUT 1, L, C
	LD A, 0x1
	INC B
	LD H, B
	SUB H, D
	JP Z, :main_init

left_loop:

	LD C, B
	OUT 1, A, C
	OUT 1, A, D
	SHL A
	JP Z, :right_start
	JP :left_loop

right_start:

	OUT 1, L, C
	LD A, 0x80
	INC B

right_loop:

	LD C, B
	OUT 1, A, C
	OUT 1, A, D
	SHR A
	JP Z, :left_start
	JP :right_loop
