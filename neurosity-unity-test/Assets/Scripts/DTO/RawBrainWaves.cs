public class RawBrainWaves
{
    public decimal[] CP3 { get; set;}
    public decimal[] C3 { get; set;}
    public decimal[] F5 { get; set;}
    public decimal[] PO3 { get; set;}
    public decimal[] PO4 { get; set;}
    public decimal[] F6 { get; set;}
    public decimal[] C4 { get; set;}
    public decimal[] CP4 { get; set;}

    public RawBrainWaves()
    {
        this.CP3 = new decimal[0];
        this.C3 = new decimal[0];
        this.F5 = new decimal[0];
        this.PO3 = new decimal[0];
        this.PO4 = new decimal[0];
        this.F6 = new decimal[0];
        this.C4 = new decimal[0];
        this.CP4 = new decimal[0];
    }

    public RawBrainWaves(
        decimal[] CP3,
        decimal[] C3,
        decimal[] F5,
        decimal[] PO3,
        decimal[] PO4,
        decimal[] F6,
        decimal[] C4,
        decimal[] CP4
    ) {
        this.CP3 = CP3;
        this.C3 = C3;
        this.F5 = F5;
        this.PO3 = PO3;
        this.PO4 = PO4;
        this.F6 = F6;
        this.C4 = C4;
        this.CP4 = CP4;
    }
}
