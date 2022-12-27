from django.utils import timezone
from django.utils.deprecation import MiddlewareMixin
from django.utils.functional import SimpleLazyObject

from .models import Authentication


HEADER_AUTH_SERVER = 'HTTP_X_AUTH_SERVER'


def auth(token):
    now = timezone.now()
    try:
        return Authentication.objects\
            .get(token=token, expire_at__gte=now)
    except:
        pass


class GameAuthenticationMiddleware(MiddlewareMixin):

    def process_request(self, request):
        auth_token = None

        if HEADER_AUTH_SERVER in request.META:
            auth_token = request.META[HEADER_AUTH_SERVER]

        request.user_server = SimpleLazyObject(lambda: auth(auth_token))
