#pragma once


const uint8_t Map[] = {
15, /*Width*/
15, /*Height*/
0, /*PlayerStart X*/
0, /*PlayerStart Y*/
15, /*Timer*/
0, /*Out of bounds Tile ID*/
1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
1,1,0,0,0,25,25,25,25,25,0,0,0,0,0,
0,1,1,0,0,0,0,0,0,25,0,0,0,0,0,
0,0,0,1,1,0,0,0,0,0,25,0,0,0,0,
0,0,0,0,1,1,0,0,0,0,25,0,0,0,0,
0,0,0,0,0,1,1,1,0,0,25,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,25,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,25,0,0,0,0,
0,0,0,25,0,0,0,0,25,25,25,0,0,0,0,
0,0,0,25,25,25,25,25,25,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,
0,
};
/*==============================*/


const uint8_t Map2[] = {
21, /*Width*/
15, /*Height*/
0, /*PlayerStart X*/
0, /*PlayerStart Y*/
15, /*Timer*/
0, /*Out of bounds Tile ID*/
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,1,1,1,0,1,1,1,0,0,0,0,
0,0,0,0,1,0,0,0,0,1,1,0,0,0,1,1,1,0,0,0,0,
0,0,0,0,1,1,0,0,0,1,0,0,0,0,1,0,1,1,0,0,0,
0,0,0,0,0,0,1,0,0,1,0,0,0,0,1,0,0,1,0,0,0,
0,0,0,0,0,0,1,0,0,1,0,0,0,0,1,0,0,1,0,0,0,
0,0,0,0,0,0,0,1,0,1,0,0,0,0,1,1,0,1,1,0,0,
0,0,0,0,0,0,0,1,1,1,0,0,0,0,0,1,0,1,1,0,0,
0,1,0,0,0,0,0,0,1,1,0,0,0,0,0,0,1,1,1,0,0,
0,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,1,0,1,0,0,
0,0,0,0,0,0,0,0,0,1,0,0,0,0,1,1,1,1,1,0,0,
0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0,0,
0,0,0,0,0,0,0,0,0,0,1,1,1,1,0,0,0,0,0,0,0,
0,
0,
};
/*==============================*/


constexpr const uint8_t numberOfMaps =2;
constexpr const uint8_t* maps[2] = {Map,Map2,};
