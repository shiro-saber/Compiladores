.assembly 'ppt' {}

.assembly extern 'pptlib' {}

.class public 'salida' extends ['mscorlib']'System'.'Object' {
	.method public static void 'inicio'() {
		.entrypoint
		ldstr "piedra"
		ldstr "papel"
		call string class ['pptlib']'ppt'.'Runtime'::'menos'(string, string)
		ldstr "papel"
		ldstr "piedra"
		call string class ['pptlib']'ppt'.'Runtime'::'mas'(string, string)
		call string class ['pptlib']'ppt'.'Runtime'::'mas'(string, string)
		call void ['mscorlib']'System'.'Console'::'WriteLine'(string)
		ret
	}
}
