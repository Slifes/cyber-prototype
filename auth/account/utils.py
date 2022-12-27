from web3.auto import w3
from eth_utils import is_hex_address
from eth_account.messages import encode_defunct


def recover_to_addr(msg, sig):
    msghash = encode_defunct(hexstr=msg)
    return w3.eth.account.recover_message(msghash, signature=sig).lower()


def validate_eth_address(value):
    return not is_hex_address(value)
