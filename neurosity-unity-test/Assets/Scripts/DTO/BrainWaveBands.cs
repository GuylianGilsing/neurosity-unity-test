public class BrainWaveBands
{
    public float alpha { get; set;}
    public float beta { get; set;}
    public float delta { get; set;}
    public float gamma { get; set;}
    public float theta { get; set;}

    public BrainWaveBands()
    {
        this.alpha = 0.0f;
        this.beta = 0.0f;
        this.delta = 0.0f;
        this.gamma = 0.0f;
        this.theta = 0.0f;
    }

    public BrainWaveBands(
        float alpha,
        float beta,
        float delta,
        float gamma,
        float theta
    ) {
        this.alpha = alpha;
        this.beta = beta;
        this.delta = delta;
        this.gamma = gamma;
        this.theta = theta;
    }
}
