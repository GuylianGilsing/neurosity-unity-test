public class BrainWaveBands
{
    public decimal[] alpha { get; set;}
    public decimal[] beta { get; set;}
    public decimal[] delta { get; set;}
    public decimal[] gamma { get; set;}
    public decimal[] theta { get; set;}

    public BrainWaveBands()
    {
        this.alpha = new decimal[0];
        this.beta = new decimal[0];
        this.delta = new decimal[0];
        this.gamma = new decimal[0];
        this.theta = new decimal[0];
    }

    public BrainWaveBands(
        decimal[] alpha,
        decimal[] beta,
        decimal[] delta,
        decimal[] gamma,
        decimal[] theta
    ) {
        this.alpha = alpha;
        this.beta = beta;
        this.delta = delta;
        this.gamma = gamma;
        this.theta = theta;
    }
}
