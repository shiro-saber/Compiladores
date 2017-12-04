.assembly 'Trillian' {}

.class public 'final_exam' extends ['mscorlib']'System'.'Object'{
	.method public static void 'main'() {
		.entrypoint
		ldc.r8 1
		ldc.r8 4.1
		dup
		add
		dup
		add
		ldc.r8 4.5
		call float64 ['mscorlib']'System'.'Math'::Max(float64, float64)
		ldc.r8 -4.2
		dup
		add
		call float64 ['mscorlib']'System'.'Math'::Max(float64, float64)
		add
		ldc.r8 -3
		add
		ldc.r8 10
		dup
		add
		call float64 ['mscorlib']'System'.'Math'::Max(float64, float64)
		call void ['mscorlib']'System'.'Console'::'WriteLine'(float64)
		ret
	}
}
