.class public abstract sealed auto ansi beforefieldinit Hw2.Calculator extends System.Object
{
  .method static public float64 Calculate(float64 value1, valuetype Hw2.CalculatorOperation operation, float64 value2) cil managed
  {
    .maxstack 2

    IL_0000: ldarg.1
    IL_0001: ldc.i4 0
    IL_0002: beq IL_0100

    IL_0010: ldarg.1
    IL_0011: ldc.i4 1
    IL_0012: beq IL_0200

    IL_0020: ldarg.1
    IL_0021: ldc.i4 2
    IL_0022: beq IL_0300

    IL_0030: ldarg.1
    IL_0031: ldc.i4 3
    IL_0032: beq IL_0400
      
    IL_0050:  ldstr   "Undefined operation"
    IL_0051:  newobj  instance void [System.Runtime]System.ArgumentOutOfRangeException::.ctor(string)
    IL_0052:  throw

    IL_0100: ldarg.0
    IL_0101: ldarg.2
    IL_0102: add
    IL_0103: ret

    IL_0200: ldarg.0
    IL_0201: ldarg.2
    IL_0202: sub
    IL_0203: ret

    IL_0300: ldarg.0
    IL_0301: ldarg.2
    IL_0302: mul
    IL_0303: ret

    IL_0400: ldarg.0
    IL_0401: ldarg.2
    IL_0402: div
    IL_0403: ret
  }
}