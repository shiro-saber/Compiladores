.assembly 'output' { }

.assembly extern 'int64lib' { }

.class public 'Test' extends ['mscorlib']'System'.'Object' {
                .method public static default int64 'main'()
        {
  .entrypoint
        .locals init ()
          ldc.i8 5
          call int64 class ['int64lib']'Int64'.'Utils'::'Printi'(int64)
          ret
  }
}
