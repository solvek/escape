using System;
using System.Collections;
using Microsoft.SPOT;

namespace EscapeGame
{
    class Melody
    {
        private int notesCount;
        private readonly int[] notes;
        private readonly int[] durations;

        private int lastDuration = 4;
        private int currentNote;
        
        public Melody(int notesCount)
        {
            this.notesCount = notesCount;
            currentNote = 0;

            notes = new int[notesCount];
            durations = new int[notesCount];
        }

        public void Play(int tempo)
        {
        #if ENABLE_SOUND
            int noteDuration;

            for (int i = 0; i < notesCount; i++)
            {                
                noteDuration = (int)durations[i];

                //BrainPad.WriteDebugMessage("Notes: " + notesCount + ", current note: " + i + ", Duration: " + noteDuration);

                noteDuration = 1000 / noteDuration;
                BrainPad.Buzzer.PlayFrequency((int)notes[i]);
                BrainPad.Wait.Milliseconds(noteDuration);
                BrainPad.Buzzer.Stop();
                BrainPad.Wait.Milliseconds(13*noteDuration/10);
            }
        #endif
        }

        public Melody N(int note, int duration)
        {
            notes[currentNote] = note;
            durations[currentNote] = duration;
            lastDuration = duration;

            currentNote++;
            return this;
        }

        public Melody N(int note)
        {
            notes[currentNote] = note;
            durations[currentNote] = lastDuration;

            currentNote++;
            return this;
        }

        public Melody N(BrainPad.Buzzer.Note note, int duration)
        {
            return N((int)note, duration);
        }

        public Melody N(BrainPad.Buzzer.Note note)
        {
            return N((int)note);
        }

        public const int NOTE_B0 = 31;
        public const int NOTE_C1 = 33;
        public const int NOTE_CS1 = 35;
        public const int NOTE_D1 = 37;
        public const int NOTE_DS1 = 39;
        public const int NOTE_E1 = 41;
        public const int NOTE_F1 = 44;
        public const int NOTE_FS1 = 46;
        public const int NOTE_G1 = 49;
        public const int NOTE_GS1 = 52;
        public const int NOTE_A1 = 55;
        public const int NOTE_AS1 = 58;
        public const int NOTE_B1 = 62;
        public const int NOTE_C2 = 65;
        public const int NOTE_CS2 = 69;
        public const int NOTE_D2 = 73;
        public const int NOTE_DS2 = 78;
        public const int NOTE_E2 = 82;
        public const int NOTE_F2 = 87;
        public const int NOTE_FS2 = 93;
        public const int NOTE_G2 = 98;
        public const int NOTE_GS2 = 104;
        public const int NOTE_A2 = 110;
        public const int NOTE_AS2 = 117;
        public const int NOTE_B2 = 123;
        public const int NOTE_C3 = 131;
        public const int NOTE_CS3 = 139;
        public const int NOTE_D3 = 147;
        public const int NOTE_DS3 = 156;
        public const int NOTE_E3 = 165;
        public const int NOTE_F3 = 175;
        public const int NOTE_FS3 = 185;
        public const int NOTE_G3 = 196;
        public const int NOTE_GS3 = 208;
        public const int NOTE_A3 = 220;
        public const int NOTE_AS3 = 233;
        public const int NOTE_B3 = 247;
        public const int NOTE_C4 = 262;
        public const int NOTE_CS4 = 277;
        public const int NOTE_D4 = 294;
        public const int NOTE_DS4 = 311;
        public const int NOTE_E4 = 330;
        public const int NOTE_F4 = 349;
        public const int NOTE_FS4 = 370;
        public const int NOTE_G4 = 392;
        public const int NOTE_GS4 = 415;
        public const int NOTE_A4 = 440;
        public const int NOTE_AS4 = 466;
        public const int NOTE_B4 = 494;
        public const int NOTE_C5 = 523;
        public const int NOTE_CS5 = 554;
        public const int NOTE_D5 = 587;
        public const int NOTE_DS5 = 622;
        public const int NOTE_E5 = 659;
        public const int NOTE_F5 = 698;
        public const int NOTE_FS5 = 740;
        public const int NOTE_G5 = 784;
        public const int NOTE_GS5 = 831;
        public const int NOTE_A5 = 880;
        public const int NOTE_AS5 = 932;
        public const int NOTE_B5 = 988;
        public const int NOTE_C6 = 1047;
        public const int NOTE_CS6 = 1109;
        public const int NOTE_D6 = 1175;
        public const int NOTE_DS6 = 1245;
        public const int NOTE_E6 = 1319;
        public const int NOTE_F6 = 1397;
        public const int NOTE_FS6 = 1480;
        public const int NOTE_G6 = 1568;
        public const int NOTE_GS6 = 1661;
        public const int NOTE_A6 = 1760;
        public const int NOTE_AS6 = 1865;
        public const int NOTE_B6 = 1976;
        public const int NOTE_C7 = 2093;
        public const int NOTE_CS7 = 2217;
        public const int NOTE_D7 = 2349;
        public const int NOTE_DS7 = 2489;
        public const int NOTE_E7 = 2637;
        public const int NOTE_F7 = 2794;
        public const int NOTE_FS7 = 2960;
        public const int NOTE_G7 = 3136;
        public const int NOTE_GS7 = 3322;
        public const int NOTE_A7 = 3520;
        public const int NOTE_AS7 = 3729;
        public const int NOTE_B7 = 3951;
        public const int NOTE_C8 = 4186;
        public const int NOTE_CS8 = 4435;
        public const int NOTE_D8 = 4699;
        public const int NOTE_DS8 = 4978;
    }
}
