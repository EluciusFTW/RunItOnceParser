using FluentAssertions;
using RioParser.Domain.HandHistories;
using Xunit;

namespace RioParser.Domain.Tests
{
    public class HandHistoryTests
    {
        private string hand =
            @"Run It Once Poker Hand #44496158:  Omaha Pot Limit (€0.05/€0.10) - 2020/05/06 22:23 UTC [2020/05/06 23:23 CET]
Table ID '44480888' 6-Max Seat #6 is the button
Seat 1: Patrick M(€9.29 in chips)
Seat 2: MiamiBlues(€10 in chips)
Seat 3: Anastasia L(€9.38 in chips)
Seat 4: Ava J(€29.49 in chips)
Seat 6: Frank L(€10 in chips)
Patrick M: posts small blind €0.05
MiamiBlues: posts big blind €0.10
*** HOLE CARDS ***
Dealt to Patrick M[Kd Ac 3h Kc]
Dealt to MiamiBlues[Ks Qs 6c 7c]
Dealt to Anastasia L[As Th Ah 4c]
Dealt to Ava J[Tc 2s 9h 8s]
Dealt to Frank L[9c Jd Qd 9s]
Anastasia L: raises €0.25 to €0.35
Ava J: folds
Frank L: calls €0.35
Patrick M: calls €0.30
MiamiBlues: calls €0.25
*** FLOP ***[5c 2h 5s]
Patrick M: checks
MiamiBlues: checks
Anastasia L: bets €0.34
Frank L: folds
Patrick M: calls €0.34
MiamiBlues: folds
*** TURN *** [5c 2h 5s]
        [8d]
        Patrick M: checks
        Anastasia L: checks
        *** RIVER *** [5c 2h 5s 8d]
        [Jc]
        Patrick M: bets €1
Anastasia L: calls €1
*** SHOWDOWN ***
Anastasia L shows[As Th Ah 4c] for Two Pairs[As Ah Jc 5s 5c]
Anastasia L collected €3.90 from pot
*** SUMMARY ***
Total pot €4.08 | Main pot €4.08 | Side pot €0.00 | Rake €0.18
Board[5c 2h 5s 8d Jc]
Seat 4: Ava J folded before Flop
Seat 6: Frank L(button) folded before Turn
Seat 2: MiamiBlues(big blind) folded before Turn
Seat 3: Anastasia L showed[As Th Ah 4c] and won €3.90 with Two Pairs A 5 and J
Seat 1: Patrick M(small blind) mucked[Kd Ac 3h Kc] and lost";

        [Fact]
        public void Extracts_Rake()
        {
            var handHistory = new HandHistory(hand);

            handHistory.Rake.Should().Be((decimal)0.18);
        }

        [Fact]
        public void Extracts_BigBlind()
        {
            var handHistory = new HandHistory(hand);

            handHistory.BigBlind.Should().Be((decimal)0.10);
        }

        [Fact]
        public void Extracts_Total()
        {
            var handHistory = new HandHistory(hand);

            handHistory.Total.Should().Be((decimal)4.08);
        }

        [Fact]
        public void Extracts_Splash()
        {
            var handHistory = new HandHistory(hand);

            handHistory.Splash.Should().Be(0);
        }

        [Fact]
        public void Extracts_Winner()
        {
            var handHistory = new HandHistory(hand);

            handHistory.Winner.Should().Be("Anastasia L");
        }
    }
}
