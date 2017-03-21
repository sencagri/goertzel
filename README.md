### Welcome to the goertzel wiki!

I created this repo to share my expericence with goertzel algorithm, caller id, signal modulation(especially fsk) and etc. I will share all my work in github. You will see the the whole development with all the nudity.

## Quick test.
If you want to test how it decodes signal data coming from just download the repo open it with visual studio (created in vs2015) select GoertzelFSKDecoder as start-up project. In the code, modify the constructor method of Form1.
    
        public Form1()
        {
            InitializeComponent();

            gd.TargetFreqs.Add(697);
            gd.TargetFreqs.Add(770);
            gd.TargetFreqs.Add(852);
            gd.TargetFreqs.Add(941);
            gd.TargetFreqs.Add(1209);
            gd.TargetFreqs.Add(1336);
            gd.TargetFreqs.Add(1477);
            gd.TargetFreqs.Add(1633);

            gd.OnGoertzelDecoded += GdOnOnGoertzelDecoded;

            gd.RunGoertzel();
        }

You can add the your desired frequencies to test the algorithm.

## All the feedbacks are highly appreciated.


