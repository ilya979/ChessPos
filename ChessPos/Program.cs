int[] bits = new int[256];

for(ulong i = 0; i < 256; i++)
{
    bits[i] = popcnt2(i);
}

Console.WriteLine(GetQueenBitboardMoves(49));

ulong GetQueenBitboardMoves(int pos)
{
    // для королевы считаем допустимые позиции офицера и ладьи

    return GetBishopBitboardMoves(pos) | GetRookBitboardMoves(pos);
}

ulong GetBishopBitboardMoves(int pos)
{
    // колонка и строчка расположения фигуры
    int row = pos / 8;
    int col = pos % 8;
    // определени точку прохождения диагоналей в строке расположения фигуры
    int cDiag1 = (row * (8 + 1))%8;
    int cDiag2 = 7*(row+1)%8;
    // теперь определим направление и величину сдвига каждой из диагоналей
    int shift1 = col - cDiag1;
    int shift2 = col - cDiag2;

    // первая и вторая диагональ
    ulong uDiag1 = 0x8040201008040201;
    ulong uDiag2 = 0x102040810204080;

    // маска необходима для отсечения перепрыгивающих через край позиций
    ulong L = 0xfefefefefefefefe;
    ulong R = 0x7f7f7f7f7f7f7f7f;

    if (shift1 > 0)
    {
        while (shift1-- > 0)
        {
            uDiag1 <<= 1;
            uDiag1 &= L;
        }
    }
    else
    {
        if (shift1 < 0)
        {
            while (shift1++ < 0)
            {
                uDiag1 >>= 1;
                uDiag1 &= R;
            }
        }
    }

    if (shift2 > 0)
    {
        while (shift2-- > 0)
        {
            uDiag2 <<= 1;
            uDiag2 &= L;
        }
    }
    else
    {
        if (shift2 < 0)
        {
            while (shift2++ < 0)
            {
                uDiag2 >>= 1;
                uDiag2 &= R;
            }
        }
    }
    
    // исклюрающее ИЛИ выбросит исходную позицию
    return uDiag1^uDiag2;
}

ulong GetKnightBitboardMoves(int pos)
{
    ulong K = 1UL << pos;
    ulong mask;

    ulong noL = 0xfefefefefefefefe;
    ulong noL2 = 0xfcfcfcfcfcfcfcfc;
    ulong noR = 0x7f7f7f7f7f7f7f7f;
    ulong noR2 = 0x3f3f3f3f3f3f3f3f;
    ulong Kl = K & noL;
    ulong Kr = K & noR;
    ulong Kl2 = K & noL2;
    ulong Kr2 = K & noR2;

    mask = 
        (Kr >> 15) | (Kl >> 17) | (Kr2 >> 6) | (Kl2 >> 10) | (Kr2 << 10) | (Kl2 << 6) | (Kr << 17) | (Kl << 15);

    return mask;
}

ulong GetKingBitboardMoves(int pos)
{
    ulong K = 1UL << pos;
    
    ulong noL = 0xfefefefefefefefe;
    ulong noR = 0x7f7f7f7f7f7f7f7f;
    ulong Kl = K & noL;
    ulong Kr = K & noR;
    ulong mask =
        (Kl << 7) | (K << 8) | (Kr << 9) |
        (Kl >> 1) |             (Kr << 1) |
        (Kl >> 9) | (K >> 8) | (Kr >> 7);


    return mask;
}

ulong GetRookBitboardMoves(int pos)
{
    int row = pos / 8;
    int col = pos % 8;
    ulong Kh = 255;
    ulong Kv = 72340172838076673UL;
    Kv <<= col;
    while (row-- > 0){
        Kh *= 256;
    }
    return Kv^Kh;
}

int popcnt(ulong mask)
{
    int cnt=0;
    while (mask > 0)
    {
        if((mask & 1)==1)  cnt ++;
        mask >>= 1;
    }
    return cnt;
}

int popcnt2(ulong mask)
{
    int cnt=0;
    while (mask > 0)
    {
        mask &= (mask - 1);
        cnt++;
    }
    return cnt;
}

int popcnt3(ulong mask)
{
    int cnt = 0;
    while (mask > 0)
    {
        cnt += bits[mask &255];
        mask >>= 8;
    }
    return cnt;
}
