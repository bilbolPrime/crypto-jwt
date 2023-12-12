// SPDX-License-Identifier: MIT

pragma solidity ^0.8.6;

import "@openzeppelin/contracts-upgradeable/proxy/utils/Initializable.sol";
import "@openzeppelin/contracts-upgradeable/access/AccessControlUpgradeable.sol";
import "@openzeppelin/contracts-upgradeable/token/ERC721/extensions/ERC721EnumerableUpgradeable.sol";

contract NFTContract is Initializable, ERC721EnumerableUpgradeable, AccessControlUpgradeable {

    bytes32 public constant NFT_ADMIN = keccak256("NFT_ADMIN");

    event NFTMinted(address indexed minter, uint256 indexed id, address indexed owner);
    event BitsUpdated(address indexed by, uint256 indexed id, uint256 oldBits, uint256 newBits);
   
    uint256 nftMinted;
    mapping(uint256 => uint256) _bits;

    function initialize () public initializer {
        __ERC721_init("NFTContract TEST", "TEST NFT");
        __AccessControl_init_unchained();

        _setupRole(DEFAULT_ADMIN_ROLE, msg.sender);
    }

    modifier restricted() {
        _restricted();
        _;
    }

    function _restricted() internal view {
        require(hasRole(NFT_ADMIN, msg.sender) || hasRole(DEFAULT_ADMIN_ROLE, msg.sender), "NA");
    }

    function getBits(uint256 id) public view returns (uint256) {
        return _bits[id];
    }

    function setBits(uint256 id, uint256 bits) public restricted {
        require(_bits[id] != bits, "Bits already set");
        emit BitsUpdated(msg.sender, id, _bits[id], bits);
        _bits[id] = bits;
    }

    function mint(address minter, uint256 bits) public restricted {
        uint256 tokenID = nftMinted++;
        _bits[tokenID] = bits;
        _mint(minter, tokenID);

        emit NFTMinted(msg.sender, tokenID, minter);
    }

    function supportsInterface(bytes4 interfaceId) public view virtual override(AccessControlUpgradeable, ERC721EnumerableUpgradeable) returns (bool) {
     return super.supportsInterface(interfaceId);
   }
}
