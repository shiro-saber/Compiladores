.assembly 'output' { }

.assembly extern 'int64lib' { }

.class public 'Test' extends ['mscorlib']'System'.'Object' {

                .field  public static int64 a
                .field  public static int64 b
                .field  public static int64 c
                .method public static
          default int64 'x'(b)
        {
              .locals init (
              x)
        ldc.i4.5
        stsfld int32 Test::c
ldc.i8 0
        call int64 class ['int64lib']'Int64'.'Utils'::'New'(int64)
        ldc.i8 34
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 70
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 117
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 110
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 99
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 116
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 105
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 111
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 110
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 32
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 120
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 92
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 110
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 34
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          call int64 class ['int64lib']'Int64'.'Utils'::'Prints'(int64)
             ldsfld int32 Test::'a'
      conv.i8
      call int64 class ['int64lib']'Int64'.'Utils'::'Printi'(int64)

          call int64 class ['int64lib']'Int64'.'Utils'::'Println'(int64)
             ldsfld int32 Test::'b'
      conv.i8
      call int64 class ['int64lib']'Int64'.'Utils'::'Printi'(int64)

          call int64 class ['int64lib']'Int64'.'Utils'::'Println'(int64)
             ldsfld int32 Test::'c'
      conv.i8
      call int64 class ['int64lib']'Int64'.'Utils'::'Printi'(int64)

          call int64 class ['int64lib']'Int64'.'Utils'::'Println'(int64)
            ret
        }
          .method public static
          default int64 'main'()
        {
        .entrypoint
              .locals init (
              x)
        ldc.i4.1
        stsfld int32 Test::a
ldc.i4.2
        stsfld int32 Test::b
ldc.i4.3
        stsfld int32 Test::c
ldc.i4.4
        stloc.0
        ldc.i8 0
        call int64 class ['int64lib']'Int64'.'Utils'::'New'(int64)
        ldc.i8 34
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 70
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 117
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 110
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 99
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 116
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 105
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 111
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 110
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 32
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 109
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 97
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 105
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 110
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 92
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 110
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          ldc.i8 34
          call int64 class ['int64lib']'Int64'.'Utils'::'Add'(int64, int64)
          call int64 class ['int64lib']'Int64'.'Utils'::'Prints'(int64)
             ldsfld int32 Test::'a'
      conv.i8
      call int64 class ['int64lib']'Int64'.'Utils'::'Printi'(int64)

          call int64 class ['int64lib']'Int64'.'Utils'::'Println'(int64)
             ldsfld int32 Test::'b'
      conv.i8
      call int64 class ['int64lib']'Int64'.'Utils'::'Printi'(int64)

          call int64 class ['int64lib']'Int64'.'Utils'::'Println'(int64)
             ldsfld int32 Test::'c'
      conv.i8
      call int64 class ['int64lib']'Int64'.'Utils'::'Printi'(int64)

          call int64 class ['int64lib']'Int64'.'Utils'::'Println'(int64)
             ldloc 'x'
                conv.i8
                call int64 class ['int64lib']'Int64'.'Utils'::'Printi'(int64)

          call int64 class ['int64lib']'Int64'.'Utils'::'Println'(int64)
            ret
        }
          }
