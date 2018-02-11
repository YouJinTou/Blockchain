(function ($) {
    $('document').ready(function () {
        $('[data-api-get]').on('click', function () {
            doGet(getUrl($(this)));
        });

        $('[data-api-post]').on('click', function () {
            var url = getUrl($(this));
            var data = getData($(this));

            doPost(url, data);
        });
    });

    function getUrl($this) {
        var area = 'Node';
        var controller = $this.data('controller');
        var action = $this.data('action');
        var url = area + '/' + controller + '/' + action;

        return url;
    }

    function getData($this) {
        var data = {};
        var $props = $this
            .closest('[data-post-container]')
            .find('[data-prop-name]');

        $props.each(function (i, p) {
            data[$(p).data('prop-name')] = $(p).val();
        });

        return data;
    }

    function doGet(url) {
        $.get(url, function (result) {
            appendResult(result);
        })
    }

    function doPost(url, data) {
        $.ajax({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            'type': 'POST',
            'url': url,
            'data': JSON.stringify(data),
            'dataType': 'json',
            'success': function (result) {
                appendResult(result);
            },
            'error': function (ex) {
                appendResult(ex.statusText);
            }
        });
    }

    function appendResult(result) {
        $('#results').empty();
        $('#results').append(JSON.stringify(result));
    }
})(jQuery);