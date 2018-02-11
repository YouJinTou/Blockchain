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
        var param = getParam($this);
        param = param ? '/' + param : '';
        var url = area + '/' + controller + '/' + action + param;

        return url;
    }

    function getParam($this) {
        var param = $this.data('param');

        if (param) {
            return $this
                .closest('[data-get-container]')
                .find('[data-param]')
                .val();
        }

        return '';
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
        $.ajax({
            url: url,
            type: 'GET',
            success: function (result) {
                appendResult(result);
            },
            error: function (ex) {
                appendResult(ex.responseJSON);
            }
        });
    }

    function doPost(url, data) {
        $.ajax({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            type: 'POST',
            url: url,
            data: JSON.stringify(data),
            dataType: 'json',
            success: function (result) {
                appendResult(result);
            },
            error: function (ex) {
                appendResult(ex.responseJSON);
            }
        });
    }

    function appendResult(result) {
        $('#results').empty();
        $('#results').append(JSON.stringify(result));
    }
})(jQuery);