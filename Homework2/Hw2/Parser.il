.class public abstract sealed auto ansi beforefieldinit Hw2.Parser extends System.Object
{
  .method public static void ParseCalcArguments(string[] args,
    [out] float64& val1,
    [out] valuetype Hw2.CalculatorOperation& oper,
    [out] float64& val2) cil managed
  {
    .maxstack 4

    IL_0001: ldarg.0
    IL_0002: call     bool Hw2.Parser::IsLengthSupported(string[])
    IL_0003: ldc.i4.0
    IL_0004: beq IL_1000
    
    IL_0010: ldarg.0
    IL_0011: ldc.i4.1
    IL_0012: ldelem.ref
    IL_0013: call     valuetype Hw2.CalculatorOperation Hw2.Parser::ParseOperation(string)
    IL_0014: ldc.i4.4
    IL_0015: beq IL_2000
    
    IL_0020: ldarg.0
    IL_0021: ldc.i4.0
    IL_0022: ldelem.ref
    IL_0023: ldarg.1
    IL_0024: call     bool [System.Runtime]System.Double::TryParse(string, float64&)

    IL_0025: ldarg.0
    IL_0026: ldc.i4.2
    IL_0027: ldelem.ref
    IL_0028: ldarg.3
    IL_0029: call bool [System.Runtime]System.Double::TryParse(string, float64&)

    IL_0030: and
    IL_0031: ldc.i4.0
    IL_0032: beq IL_3000

    IL_0100: ldarg.2
    IL_0101: ldarg.0
    IL_0102: ldc.i4.1
    IL_0103: ldelem.ref
    IL_0104: call     valuetype Hw2.CalculatorOperation Hw2.Parser::ParseOperation(string)
    IL_0105: stind.i4
    IL_0106: ret

    IL_1000:  ldstr   "Number of arguments must be 3"
    IL_1001:  newobj  instance void [System.Runtime]System.ArgumentException::.ctor(string)
    IL_1002:  throw

    IL_2000:  ldstr   "Operation is undefined"
    IL_2001:  newobj  instance void [System.Runtime]System.InvalidOperationException::.ctor(string)
    IL_2002:  throw

    IL_3000:  ldstr   "One of the values is invalid"
    IL_3001:  newobj  instance void [System.Runtime]System.ArgumentException::.ctor(string)
    IL_3002:  throw
  }

  .method private static bool IsLengthSupported(string[] args) cil managed
  {
    .maxstack 2
        
    ldarg.0
    ldlen
    ldc.i4.3
    ceq
    ret
  }

  .method private static valuetype Hw2.CalculatorOperation ParseOperation(string arg)
  {
    .maxstack 2

    IL_0001: ldarg.0
    IL_0002: ldstr "+"
    IL_0003: call    bool [System.Runtime]System.String::op_Equality(string,string)
    IL_0004: brtrue.s IL_0100

    IL_0005: ldarg.0
    IL_0006: ldstr "-"
    IL_0007: call    bool [System.Runtime]System.String::op_Equality(string,string)
    IL_0008: brtrue.s IL_0200

    IL_0009: ldarg.0
    IL_0010: ldstr "*"
    IL_0011: call    bool [System.Runtime]System.String::op_Equality(string,string)
    IL_0012: brtrue.s IL_0300

    IL_0013: ldarg.0
    IL_0014: ldstr "/"
    IL_0015: call    bool [System.Runtime]System.String::op_Equality(string,string)
    IL_0016: brtrue.s IL_0400

    IL_0050: ldc.i4.4
    IL_0051: ret

    IL_0100: ldc.i4.0
    IL_0101: ret

    IL_0200: ldc.i4.1
    IL_0201: ret

    IL_0300: ldc.i4.2
    IL_0301: ret

    IL_0400: ldc.i4.3
    IL_0401: ret
  }
}