# Crypto JWT

A simple JWT integration with NFTs (ERC721 standard)

# Local Chain

1. Install [Ganache](https://www.trufflesuite.com/ganache)

## Local Chain Setup

1. `npm install` (inside ./Chain)
1. `truffle compile --all` (inside ./Chain)
1. `truffle deploy` (inside ./Chain)
1. `mint` to mint NFTs to any address
1. `setBits` to update an NFT's bits


```
let nftContract = await NFTContract.deployed()
await nftContract.mint('some address', some bit value)
await nftContract.setBits(nft id, new bits value)
```

# App Settings Setup

1. Update ChainInfo data to match local chain or remote chain
1. Assign any private key

## API 

1. `~/challenge` request a challenge to sign.


```
{
    "account":"0x4B4AABaf30921059e3baD5Ab10f8d19d88cccC60",
    "nftId": 0
}
```

```
{
    "message": "0x4B4AABaf30921059e3baD5Ab10f8d19d88cccC60 owns 0 with nonce 1367492758"
}
```

2. `~/login` submits a signature to login.
The response includes the bits of that nft representing whatever access it should have.
```
{
    "account":"0x4B4AABaf30921059e3baD5Ab10f8d19d88cccC60",
    "nftId": 0,
    "signature": "cf0f0099d24e12dd41213856562cac4928f6eaac4c98bce6b99e05ce7dfedb495cd243ca401dd899a7494c1443bb4a102ad92fbb8c310fbc0a06919126e921ed1c"
}
```

```
{
    "account": "0x4B4AABaf30921059e3baD5Ab10f8d19d88cccC60",
    "nftId": 0,
    "bits": 3,
    "token": "a jwt token"
}
```

3. `~/test` a get request which includes Authorization header having the jwt token.
The response is a blank 200 ok code but BE would have access to the account, nft and bits.

The account ownership of the NFT and the bits are compared against the chain. 

A future enhancement would be to implement caching and a scraper that follows the contract on chain to clear cache when need be.

# Useful Links

You can sign using [My Ether Wallet](https://www.myetherwallet.com/)

# Version History

1. 2023-12-12: Initial release v1.0.0 

# Disclaimer

This implementation was made for educational / training purposes only.

# License

License is [MIT](https://en.wikipedia.org/wiki/MIT_License)

# MISC

Birbia is coming
