from django_ethereum_events.chainevents import AbstractEventReceiver

from account.models import Token


class EventTokenTransfer(AbstractEventReceiver):

    def save(self, decoded_event):
        print(decoded_event)

        _from = decoded_event['args']['from']
        _to = decoded_event['args']['to']
        _token_id = decoded_event['args']['tokenId']

        if _from == '0x0000000000000000000000000000000000000000':
            print("----- MINTED ------")
            Token.objects.create(
                owner=_to,
                token_id=_token_id
            )
        
        else:
            print("----- TRANSFERED ------")
            Token.objects\
                .filter(owner=_from, token_id=_token_id)\
                .update(owner=_to)
